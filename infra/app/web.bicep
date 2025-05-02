param name string
param location string = resourceGroup().location
param tags object = {}
param apiBaseUrl string
param branch string
param sku object

// Frontend environment variables
param reactAppClientId string
param reactAppAuthority string
param reactAppScope string = 'api://insurance-quotes-api/Quotes.Read'

resource web 'Microsoft.Web/staticSites@2022-09-01' = {
  name: name
  location: location
  tags: union(tags, {
    'azd-service-name': 'web'
  })
  sku: sku
  properties: {
    buildProperties: {
      appLocation: 'frontend/InsuranceQuotes.UI'
      outputLocation: 'build'
      skipGithubActionWorkflowGeneration: false
    }
    allowConfigFileUpdates: true
    stagingEnvironmentPolicy: 'Enabled'
    enterpriseGradeCdnStatus: 'Disabled'
  }
}

resource webSettings 'Microsoft.Web/staticSites/config@2022-09-01' = {
  parent: web
  name: 'appsettings'
  properties: {
    REACT_APP_API_URL: apiBaseUrl
    REACT_APP_CLIENT_ID: reactAppClientId
    REACT_APP_AUTHORITY: reactAppAuthority
    REACT_APP_REDIRECT_URI: 'https://${web.properties.defaultHostname}'
    REACT_APP_API_SCOPE: reactAppScope
  }
}

output uri string = 'https://${web.properties.defaultHostname}'
output name string = web.name
