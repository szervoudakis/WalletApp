# WalletApp

A sample .NET 8+ application demonstrating clean architecture, integration with the European Central Bank (ECB) for currency exchange rates, and wallet management with balance adjustments.

## Features
- **ECB Gateway**: Fetches and parses daily exchange rates from the European Central Bank.
- **Background Jobs**: Periodically updates exchange rates in the database using MERGE statements.
- **Wallet Management**:
  - Create wallets with a base currency
  - Retrieve balances with currency conversion
  - Adjust balances using different strategies (Add, Subtract, Force Subtract)
- **Design Patterns**:
  - Dependency Injection
  - CQRS pattern separation operations 
- **Extras (optional)**:
  - Caching of latest exchange rates
  - Rate limiting middleware
- **Unit Tests** with xUnit

## Tech Stack
- .NET 8+
- Entity Framework Core
- Hangfire (for scheduled jobs)
- Autofac (for dependency injection)
- xUnit (for testing)
- Redis (for caching)
## Getting Started
1. Clone the repository
2. Set up database connection in `appsettings.json`
3. Run migrations (`dotnet ef database update`)
4. Start the API (`dotnet run --project WalletApp.Api`)
