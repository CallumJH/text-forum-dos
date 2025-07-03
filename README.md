# text-forum-dos
An updated approach towards how I would make a neat text forum app


# What can you do with the project currently

Unfortunately I did not finish the desired outcome of the project within the time frame.
I have missed the following.

- Request to Response wiring for testing
  - Postman collection for these requests
- Connected auth
- Unit tests
- Integration tests
- Mocking
- Session management for at most 100 users
- Features required
  - Create post
  - Like post
  - Comment on post
  - Flag / Tag posts
  - Moderation 
  - Login
  - Decent documentation for running the project
##

# The current project

This project is built using Clean Architecture principles with Domain-Driven Design (DDD) patterns in .NET. 
The solution is structured into several key projects that separate concerns and maintain a clear dependency flow.

## Architecture Overview

The solution follows a Clean Architecture approach with the following layers:

1. **Core (Domain Model)**
   - Contains all business entities, aggregates, and domain logic
   - Defines interfaces for infrastructure concerns
   - Holds domain specific events and event handlers
   - Located in `src/Whispers.Chat.Core`

2. **Infrastructure**
   - Implements data access using Entity Framework Core
   - Contains database migrations and configurations
   - Located in `src/Whispers.Chat.Infrastructure`

3. **Use Cases (Application Layer)**
   - Implements application-specific business rules
   - Organizes features using CQRS pattern (Commands/Queries)
   - Provides a clean API for the web layer
   - Located in `src/Whispers.Chat.UseCases`

4. **Web API**
   - Handles HTTP requests and responses
   - Implements API endpoints using FastEndpoints
   - Configures middleware and services
   - Located in `src/Whispers.Chat.Web`

## Technical Stack

- **Framework**: .NET 9.0
- **API Design**: FastEndpoints for clean, performant endpoints
- **Data Access**: Entity Framework Core with SQLite
- **Authentication**: ASP.NET Core Identity (planned)
- **Logging**: Serilog with file and console sinks
- **Documentation**: Swagger/OpenAPI
- **Monitoring**: OpenTelemetry integration
- **Service Discovery**: Built-in .NET service discovery

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A code editor (recommended: Visual Studio 2022 or VS Code)
- SQLite (included in the project, no separate installation needed)

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/text-forum-dos.git
   cd text-forum-dos
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore Whispers.Chat.slnx
   ```

3. **Set Up the Database**
   The project uses SQLite with Entity Framework Core. The database will be automatically created on first run, but you can manually initialize it with:
   ```bash
   cd CD Whispers.Chat/src/Whispers.Chat.Web
   dotnet ef database update -c AppDbContext -p ../Whispers.Chat.Infrastructure/Whispers.Chat.Infrastructure.csproj -s Whispers.Chat.Web.csproj
   ```

4. **Run the Application**
   ```bash
   dotnet run --project src/Whispers.Chat.Web/Whispers.Chat.Web.csproj
   ```
   The application will start and be available at:
   - API: https://localhost:57679
   - Swagger Documentation: https://localhost:57679

- Side note, this might be better to run by opening the solution in Visual Studio

### Development Workflow

1. **Building the Solution**
   ```bash
   dotnet build Whisper.Chat.slnx
   ```

2. **Database Migrations**
   When making changes to the data model:
   ```bash
   cd src/Whispers.Chat.Web
   dotnet ef migrations add [MigrationName]
   dotnet ef database update
   ```

### Configuration

- The main configuration file is located at `src/Whispers.Chat.Web/appsettings.json`
- For development, use `appsettings.Development.json`
- Key settings include:
  - Database connection string
  - Logging configuration
  - Server settings

### Logging

- Logs are written to both console and file
- Log files are created in the application root with format `log[DATE].txt`
- Configure log levels in `appsettings.json` under the "Serilog" section

## Final notes

I am continuing to work on this repo until the listed items are done above on a separate branch named V2 if you would
be interested in tracking its further progress.
