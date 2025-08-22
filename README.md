# WalletApp

A sample .NET 5+ application demonstrating clean architecture, integration with the European Central Bank (ECB) for currency exchange rates, and wallet management with balance adjustments.

## Features
- **ECB Gateway**: Fetches and parses daily exchange rates from the European Central Bank.
- **Background Jobs**: Periodically updates exchange rates in the database using MERGE statements.
- **Wallet Management**:
  - Create wallets with a base currency
  - Retrieve balances with currency conversion
  - Adjust balances using different strategies (Add, Subtract, Force Subtract)
- **Design Patterns**:
  - Strategy Pattern for balance adjustments
  - Factory/Abstractions for extensibility
- **Extras (optional)**:
  - Caching of latest exchange rates
  - Rate limiting middleware
- **Unit Tests** with xUnit

## Tech Stack
- .NET 5+
- Entity Framework Core
- Quartz.NET (for scheduled jobs)
- Autofac (for dependency injection)
- xUnit (for testing)

## Getting Started
1. Clone the repository
2. Set up database connection in `appsettings.json`
3. Run migrations (`dotnet ef database update`)
4. Start the API (`dotnet run --project WalletApp.Api`)
