# gRPC Discount Code System

This is a simple client-server application built with **.NET**, **gRPC**, **Entity Framework Core**, and **SQLite**.  
It allows you to generate and use discount codes via gRPC communication.

## Project Structure

```
GrpcDiscountSolution/
├── DiscountServer/     - gRPC server with EF Core
├── DiscountClient/     - Console client app
├── DiscountShared/     - Shared .proto definitions
├── DiscountStorage/    - Persistence layer (EF Core + SQLite)
├── Discount.Tests/     - Unit tests for server logic
└── discounts.db        - SQLite database (created automatically)
```

## How to Run

### 1. Clone the repository

```bash
git clone https://github.com/your-username/grpc-discount-system.git
cd grpc-discount-system
```

### 2. Create the database

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project DiscountStorage --startup-project DiscountServer
dotnet ef database update --project DiscountStorage --startup-project DiscountServer
```

### 3. Run All Tests

```bash
dotnet test
```

### 4. Run the server

```bash
dotnet run --project DiscountServer
```

The server listens on: `http://localhost:5000`

### 5. Run the client (in a new terminal)

```bash
dotnet run --project DiscountClient
```

## Notes

- Generated codes are stored in `discounts.db` (SQLite)
- You can view/edit the database with [DB Browser for SQLite](https://sqlitebrowser.org/)
- Use real codes from the DB when prompted in the client

## Code Use Result

- `0` → Success
- `1` → Code not found
- `2` → Code already used

## Requirements

- .NET 8 SDK or newer
- EF Core CLI (`dotnet-ef`)
