# Insurance Quotes Project

This project provides a user interface to fetch auto insurance quotes using an ASP.NET Core backend and React frontend.

## Project Structure

```
insurance-quotes/
├── backend/
│   └── InsuranceQuotes.Api/        # ASP.NET Core 8.0 API
│       ├── Controllers/            # API Controllers
│       ├── Models/                 # Data Models
│       ├── Services/               # Business Logic
│       └── Properties/             # Launch Settings
├── frontend/
│   └── InsuranceQuotes.UI/        # React Frontend
│       ├── public/                 # Static files
│       └── src/                    # React components
├── infra/                         # Infrastructure files
└── docs/                          # Documentation
```

## Features

- **Quote Management**: API endpoints for retrieving auto insurance quotes
- **Sample Data**: Includes realistic sample quotes for testing
- **Swagger Documentation**: Interactive API documentation
- **CORS Support**: Configured for cross-origin requests
- **Azure Ready**: Configured for Azure App Service deployment

## API Endpoints

- `GET /api/quotes`: Retrieve all available insurance quotes
- `GET /api/quotes/{id}`: Get a specific quote by ID

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (version 14 or later)
- [Azure Developer CLI (azd)](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)
- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Microsoft Entra ID Setup

Before deployment, register two applications in Microsoft Entra ID (Azure AD):

1. **API Application**
   - Create new registration for "Insurance Quotes API"
   - Add app role: "Quote.Reader"
   - Save the application (client) ID

2. **Client Application**
   - Create new registration for "Insurance Quotes UI"
   - Set platform to "Single-page application (SPA)" with redirect URI: `http://localhost:3000`
   - Add API permission for the API app's "Quote.Reader" role
   - Grant admin consent for the permissions

Save both application (client) IDs and tenant ID for the deployment.

## Local Development

1. Clone the repository
2. Backend Setup:
   ```bash
   cd backend/InsuranceQuotes.Api
   dotnet restore
   dotnet build
   ```

3. Frontend Setup:
   ```bash
   cd frontend/InsuranceQuotes.UI
   npm install
   ```

## Running Locally

1. Start the Backend:
   ```bash
   cd backend/InsuranceQuotes.Api
   dotnet run
   ```
   The API will be available at:
   - `https://localhost:7035` (HTTPS)
   - `http://localhost:5228` (HTTP)
   - Swagger UI: `https://localhost:7035/swagger`

2. Start the Frontend:
   ```bash
   cd frontend/InsuranceQuotes.UI
   npm start
   ```
   The React application will open in your browser at `http://localhost:3000`

## Azure Deployment

The project uses Azure Developer CLI (azd) for streamlined deployment to Azure. The infrastructure is defined as code using Bicep templates in the `infra/` directory.

### Prerequisites for Deployment

1. Login to Azure:
   ```bash
   azd auth login
   ```

2. Configure the deployment:
   ```bash
   azd init
   ```
   This will initialize your environment and create necessary Azure resources.

### Deployment Steps

1. Preview the changes that will be made to your Azure environment:
   ```bash
   azd provision --preview
   ```
   Review the changes to ensure they match your expectations.

2. Deploy the entire application stack:
   ```bash
   azd up
   ```
   This command will:
   - Provision all required Azure resources
   - Build and package your application
   - Deploy the backend API to Azure App Service
   - Deploy the frontend to Azure Static Web Apps
   - Configure necessary settings and connections

### Post-Deployment

After successful deployment, `azd` will output:
- The URLs for your deployed applications
- Resource group information
- Monitoring and logging links

### Environment Management

- To create additional environments:
  ```bash
  azd env new [environment-name]
  ```

- To switch between environments:
  ```bash
  azd env select [environment-name]
  ```

- To list all environments:
  ```bash
  azd env list
  ```

### Monitoring and Troubleshooting

The deployment includes Application Insights for monitoring. Access logs and metrics through:
- Azure Portal > Your Resource Group > Application Insights
- Or use the URL provided in the `azd up` output

## Contributing

Please follow the standard pull request process and ensure your changes include both unit tests and documentation updates.

## License

This project is licensed under the MIT License. See the LICENSE file for details.