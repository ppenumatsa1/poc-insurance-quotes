param name string
param location string = resourceGroup().location
param tags object = {}

param applicationInsightsName string
param appServicePlanId string
param keyVaultName string
param allowedOrigins array = []

param azureAdInstance string
param azureAdDomain string
param azureAdTenantId string
param azureAdClientId string
param azureAdScope string

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource api 'Microsoft.Web/sites@2022-09-01' = {
  name: name
  location: location
  tags: union(tags, {
    'azd-service-name': 'api'  // Adding required tag for azd
  })
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    siteConfig: {
      cors: {
        allowedOrigins: allowedOrigins
        supportCredentials: allowedOrigins[0] != '*'  // Only enable credentials when not using wildcard
      }
      netFrameworkVersion: 'v8.0'
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsights.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'AZURE_KEY_VAULT_ENDPOINT'
          value: keyVault.properties.vaultUri
        }
        {
          name: 'AZURE_AD_INSTANCE'
          value: azureAdInstance
        }
        {
          name: 'AZURE_AD_DOMAIN'
          value: azureAdDomain
        }
        {
          name: 'AZURE_AD_TENANT_ID'
          value: azureAdTenantId
        }
        {
          name: 'AZURE_AD_CLIENT_ID'
          value: azureAdClientId
        }
        {
          name: 'AZURE_AD_SCOPE'
          value: azureAdScope
        }
        {
          name: 'AZURE_AD_VALID_ISSUER_1'
          value: '${azureAdInstance}${azureAdTenantId}/v2.0'
        }
        {
          name: 'AZURE_AD_VALID_ISSUER_2'
          value: 'https://sts.windows.net/${azureAdTenantId}/'
        }
      ]
    }
  }
}

resource apiKeyVaultAccess 'Microsoft.KeyVault/vaults/accessPolicies@2022-07-01' = {
  parent: keyVault
  name: 'add'
  properties: {
    accessPolicies: [
      {
        objectId: api.identity.principalId
        tenantId: api.identity.tenantId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}

output uri string = 'https://${api.properties.defaultHostName}'
output id string = api.id
output name string = api.name
