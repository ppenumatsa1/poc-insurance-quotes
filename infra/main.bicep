targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment which is used to generate a short unique hash used in all resources.')
param environmentName string

@description('Primary location for all resources')
param location string = 'northcentralus'

param apiServiceName string = ''
param webServiceName string = ''

@description('Id of the user or app to assign application roles')
param principalId string = ''

// Tags that should be applied to all resources.
param tags object = {}

// The branch that all resources are deployed from
param branch string = 'dev'

@minLength(1)
@maxLength(90)
@description('Name of the resource group')
param resourceGroupName string = 'rg-calcas1'

@description('Increment number to avoid naming conflicts with soft-deleted resources')
param deploymentIncrement int = 1

// Azure AD configuration parameters with default values from .env.local
param azureAdInstance string = 'https://login.microsoftonline.com/'
param azureAdDomain string = 'microsoft.com'
param azureAdTenantId string = tenant().tenantId
param azureAdClientId string
param azureAdScope string = 'api://insurance-quotes-api/Quotes.Read'

// Frontend configuration parameters
param reactAppClientId string = '4bfc2c61-f3c2-4c10-a8c2-903847af4a1f'
param reactAppAuthority string = 'https://login.microsoftonline.com/${azureAdTenantId}'

// resourceToken is a unique string that is used to generate unique names for resources
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var uniqueResourceToken = '${resourceToken}${deploymentIncrement}'

// SKU configurations for all services
param appServicePlanSku object = {
  name: 'P0v3'
  tier: 'PremiumV3'
}

param logAnalyticsSku object = {
  name: 'PerGB2018'
}

param staticWebAppSku object = {
  name: 'Standard'
  tier: 'Standard'
}

// Organize resources in a resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
  tags: tags
}

// Add log analytics workspace for app insights
module monitoring './core/monitor/monitoring.bicep' = {
  name: 'monitoring'
  scope: rg
  params: {
    location: location
    tags: tags
    logAnalyticsName: 'log-${resourceToken}'
    applicationInsightsName: 'appi-${resourceToken}'
    sku: logAnalyticsSku
  }
}

// Create an App Service Plan to group applications under the same payment plan and SKU
module appServicePlan './core/host/appserviceplan.bicep' = {
  name: 'appserviceplan'
  scope: rg
  params: {
    name: 'plan-${resourceToken}'
    location: location
    tags: tags
    sku: appServicePlanSku
  }
}

// The application backend
module api './app/api.bicep' = {
  name: 'api'
  scope: rg
  params: {
    name: !empty(apiServiceName) ? apiServiceName : 'api-${resourceToken}'
    location: location
    tags: tags
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    appServicePlanId: appServicePlan.outputs.id
    keyVaultName: keyVault.outputs.name
    allowedOrigins: [ '*' ]  // We'll update this after web deployment
    azureAdInstance: azureAdInstance
    azureAdDomain: azureAdDomain
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    azureAdScope: azureAdScope
  }
}

// The application frontend
module web './app/web.bicep' = {
  name: 'web'
  scope: rg
  params: {
    name: !empty(webServiceName) ? webServiceName : 'web-${resourceToken}'
    location: 'centralus'  // Changed to centralus for Static Web Apps
    tags: tags
    apiBaseUrl: '${api.outputs.uri}/api'
    branch: branch
    sku: staticWebAppSku
    reactAppClientId: reactAppClientId
    reactAppAuthority: reactAppAuthority
    reactAppScope: azureAdScope
  }
}

// Store secrets in a keyvault
module keyVault './core/security/keyvault.bicep' = {
  name: 'keyvault'
  scope: rg
  params: {
    name: 'kv-calcas-${uniqueResourceToken}'
    location: location
    tags: tags
    principalId: principalId
  }
}

// Update API CORS settings after web app is created
module apiCorsUpdate './app/api.bicep' = {
  name: 'apiCorsUpdate'
  scope: rg
  params: {
    name: !empty(apiServiceName) ? apiServiceName : 'api-${resourceToken}'
    location: location
    tags: tags
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    appServicePlanId: appServicePlan.outputs.id
    keyVaultName: keyVault.outputs.name
    allowedOrigins: [ web.outputs.uri ]
    azureAdInstance: azureAdInstance
    azureAdDomain: azureAdDomain
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    azureAdScope: azureAdScope
  }
}

output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
output AZURE_RESOURCE_GROUP string = rg.name

output API_URI string = api.outputs.uri
output WEB_URI string = web.outputs.uri
