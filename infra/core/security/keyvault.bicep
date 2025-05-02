param name string
param location string = resourceGroup().location
param tags object = {}
param principalId string = ''

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: false
    enabledForTemplateDeployment: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    accessPolicies: !empty(principalId) ? [
      {
        objectId: principalId
        permissions: {
          secrets: [
            'all'
          ]
        }
        tenantId: subscription().tenantId
      }
    ] : []
  }
}

output name string = keyVault.name
output endpoint string = keyVault.properties.vaultUri
