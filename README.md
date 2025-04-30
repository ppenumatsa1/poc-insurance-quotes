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
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)

## Installation

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

## Running the Application

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

## Building for Production

1. Build Backend:
   ```bash
   cd backend/InsuranceQuotes.Api
   dotnet publish -c Release
   ```

2. Build Frontend:
   ```bash
   cd frontend/InsuranceQuotes.UI
   npm run build
   ```

## Deployment

### Azure Deployment
The project is configured for Azure App Service deployment using Azure Developer CLI (azd):

```bash
azd up
```

This will deploy:
- Backend API to Azure App Service
- Frontend to Azure Static Web Apps (if configured)

## Contributing

Please follow the standard pull request process and ensure your changes include both unit tests and documentation updates.

## License

This project is licensed under the MIT License. See the LICENSE file for details.