# Stock Portfolio Management API

A comprehensive RESTful API for managing stock portfolios built with .NET Core 8.0, Entity Framework, and SQLite database.

## Features

- **Stock Management**: CRUD operations for stock master data
- **Portfolio Management**: Create and manage multiple portfolios
- **Transaction Management**: Buy and sell stock transactions
- **Real-time Calculations**: Automatic calculation of portfolio values, gains/losses
- **Search & Filter**: Search stocks by symbol, company name, sector, or industry
- **RESTful API**: Clean REST endpoints with proper HTTP status codes
- **Data Validation**: Comprehensive input validation and error handling
- **Soft Delete**: Safe deletion with soft delete functionality

## Architecture

The application follows the **Controller-Service-Repository-Model** pattern:

- **Models**: Domain entities and DTOs
- **Repositories**: Data access layer with Entity Framework
- **Services**: Business logic layer
- **Controllers**: API endpoints and request handling

## Technology Stack

- .NET Core 8.0
- Entity Framework Core 9.0
- SQLite Database
- Swagger/OpenAPI for documentation
- CORS enabled for frontend integration

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code (optional)

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore packages:
   ```bash
   dotnet restore
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7000` (or the port shown in the console).

### Database

The application uses SQLite database with automatic database creation and seed data. The database file `StockPortfolio.db` will be created in the project root on first run.

## API Endpoints

### Stocks

- `GET /api/stocks` - Get all stocks
- `GET /api/stocks/{id}` - Get stock by ID
- `GET /api/stocks/symbol/{symbol}` - Get stock by symbol
- `GET /api/stocks/search?q={query}` - Search stocks
- `POST /api/stocks` - Create new stock
- `PUT /api/stocks/{id}` - Update stock
- `PUT /api/stocks/{id}/price` - Update stock price
- `DELETE /api/stocks/{id}` - Delete stock (soft delete)

### Portfolios

- `GET /api/portfolios` - Get all portfolios
- `GET /api/portfolios/{id}` - Get portfolio by ID
- `GET /api/portfolios/{id}/stocks` - Get portfolio with stocks
- `GET /api/portfolios/owner/{owner}` - Get portfolios by owner
- `POST /api/portfolios` - Create new portfolio
- `PUT /api/portfolios/{id}` - Update portfolio
- `PUT /api/portfolios/{id}/recalculate` - Recalculate portfolio values
- `DELETE /api/portfolios/{id}` - Delete portfolio (soft delete)

### Transactions

- `GET /api/transactions` - Get all transactions
- `GET /api/transactions/{id}` - Get transaction by ID
- `GET /api/transactions/portfolio/{portfolioId}` - Get transactions by portfolio
- `GET /api/transactions/stock/{stockId}` - Get transactions by stock
- `GET /api/transactions/date-range?startDate={date}&endDate={date}` - Get transactions by date range
- `GET /api/transactions/quantity/{portfolioId}/{stockId}` - Get current quantity
- `POST /api/transactions` - Create new transaction (buy/sell)
- `PUT /api/transactions/{id}` - Update transaction
- `DELETE /api/transactions/{id}` - Delete transaction (soft delete)

## Data Models

### Stock

- Symbol (unique)
- Company Name
- Sector, Industry
- Exchange, Currency
- Current Price, Market Cap, Volume
- Last Updated timestamp

### Portfolio

- Name, Description
- Owner
- Total Value, Total Cost
- Total Gain/Loss and Percentage

### Transaction

- Type (Buy/Sell)
- Quantity, Price
- Total Amount, Commission, Fees
- Net Amount
- Transaction Date, Notes

### PortfolioStock (Holdings)

- Quantity, Average Price
- Current Price, Total Value
- Total Cost, Gain/Loss
- Gain/Loss Percentage

## Sample Data

The application comes with seed data including:

- 6 sample stocks (AAPL, MSFT, GOOGL, TSLA, AMZN, NVDA)
- 3 sample portfolios
- Sample transactions and holdings

## Error Handling

The API includes comprehensive error handling:

- Global exception middleware
- Custom exception types
- Proper HTTP status codes
- Detailed error messages
- Input validation

## Development

### Project Structure

```
StockPortfolioAPI/
├── Controllers/          # API Controllers
├── Data/                # DbContext and Seed Data
├── Middleware/          # Global Exception Handling
├── Models/              # Domain Models and DTOs
│   ├── DTOs/           # Data Transfer Objects
│   └── Exceptions/     # Custom Exceptions
├── Repositories/        # Data Access Layer
├── Services/           # Business Logic Layer
└── Program.cs          # Application Configuration
```

### Adding New Features

1. Create domain model in `Models/`
2. Create DTOs in `Models/DTOs/`
3. Add repository interface and implementation
4. Add service interface and implementation
5. Create controller
6. Register services in `Program.cs`

## Testing

You can test the API using:

- Swagger UI (available at `/swagger` in development)
- Postman or similar API testing tools
- The provided sample data for testing

## License

This project is for educational and demonstration purposes.
