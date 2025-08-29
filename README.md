<div align="center"> 
<p>
  <a href="https://github.com/pankaj085/LotusAscend">
    <img src="DocsAssets/LotusAscend_Logo.svg" alt="LotusAscend" width="350"/>
  </a>
</p> 

  ![.NET](https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet)
  ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-12-4169E1?style=for-the-badge&logo=postgresql)
  ![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)
  ![Status](https://img.shields.io/badge/status-active-brightgreen.svg?style=for-the-badge)  
</div>

# **LotusAscend** - A Modern Loyalty & Rewards API
LotusAscend is a complete backend and frontend solution for a modern customer loyalty program.  
The name **"Ascend"** captures the journey of a customer as they earn points and move up by getting redeem points, highlighting growth and progress within the program.

This project includes a **secure RESTful API** built with **.NET 8** and a **clean, responsive vanilla JavaScript frontend**.  
It’s designed to handle **member registration, login, points management, and coupon redemption** with unique, generated codes.

---

## ✨ Features
- **Secure Authentication**: JWT-based authentication with mobile number + OTP verification.  
- **Points Management**: Earn points based on purchase amounts, check balances.  
- **Coupon Redemption**: Redeem points for discount coupons with unique codes.  
- **Clean Architecture**: Modular, layered backend promoting separation of concerns.  
- **Responsive Frontend**: Lightweight SPA-style frontend with HTML, CSS, and Vanilla JS.  
- **API Documentation**: Integrated Swagger UI for interactive testing.

### Database Schema & API Flow Chart

**Database Schema:** For a detailed breakdown of the database tables, columns, relationships, and sample data, see the **[DATABASE Documentation](DATABASE_SCHEMA.md)**.

**API Flow Diagram:** For a detailed breakdown & Flow of the API Work Flow, see the **[API FLOW DIAGRAM](/DocsAssets/la_flow_chart.png)**.

---
## 🧪 API Testing with Postman

To make testing the LotusAscend API as easy as possible, **My complete Postman collection is available, Click the button below** to fork the collection to your own Postman workspace.

[![Run in Postman](https://run.pstmn.io/button.svg)](https://www.postman.com/mrlotus/workspace/lotusascend/collection/43877835-77b66af7-f6da-4b1e-a040-f300518061ab?action=share&creator=43877835)

>After forking, remember to set the `baseUrl` variable in the collection's settings to your local environment (e.g., `http://localhost:5245`).
---

## 🛠️ Technologies & Packages

Built on **.NET 8** using key NuGet packages:  

- `Microsoft.EntityFrameworkCore`: EF Core ORM for database communication  
- `Microsoft.EntityFrameworkCore.Design`: Design-time tools for migrations  
- `Microsoft.EntityFrameworkCore.Tools`: CLI tools for schema management  
- `Npgsql.EntityFrameworkCore.PostgreSQL`: PostgreSQL EF Core provider  
- `Microsoft.AspNetCore.Authentication.JwtBearer`: JWT auth middleware  
- `System.IdentityModel.Tokens.Jwt`: Token creation & signing  
- `Microsoft.AspNetCore.OpenApi`: Swagger/OpenAPI docs  
- `AutoMapper.Extensions.Microsoft.DependencyInjection`: DTO ↔ entity mapping  

> Note: `FluentValidation.AspNetCore` was considered but not used. Instead, built-in **Data Annotations** (`[Required]`, `[StringLength]`) are used.

---

## 📂 Project Structure
The project follows a clean and logical structure to promote separation of concerns.

```bash
LotusAscend/
│
├── AppDataContext/           # EF Core DbContext for database interaction
├── Contracts/                # DTOs for API request/response models
├── Controllers/              # API endpoints that handle HTTP requests
├── Interfaces/               # Service abstractions (contracts for business logic)
├── Middleware/               # Custom middleware (e.g., global exception handling)
├── Models/                   # EF Core entity models representing database tables
├── Services/                 # Concrete implementation of business logic
│
├── Frontend/                 # Simple Static - HTML + CSS & Vanilla JS Frontend
│   │  ├── assets/
│   │  │   ├── css/
│   │  │   ├── images/
│   │  │   └── js/
│   ├── dashboard.html
│   └── index.html
│
├── appsettings.json          # Configuration for database, JWT, etc.
└── Program.cs                # Application startup, DI container, and pipeline setup
```
## ⚙️ Getting Started

Follow these instructions to get a local copy of LotusLedger up and running on your machine.

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Git](https://git-scm.com/)
-   A running instance of [PostgreSQL](https://www.postgresql.org/download/)

### 1. Clone the Repository

```bash
git clone https://github.com/pankaj085/LotusAscend.git
cd LotusAscend
````

### 2\. Install NuGet Packages

This project relies on several NuGet packages. Run the following commands to install them:

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.8
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8         
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.8
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.1.2

```

### 3\. Configure the Database & JWT

Open `appsettings.Development.json` and update the `DefaultConnection` string with your PostgreSQL credentials.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=DATABASE_NAME;Username=USERNAME;Password=PASSWORD"
  },
    "Jwt": {
    "Key": "ThisIsMySuperSecretKeyForLoyaltyAPIProjectAndItIsVeryLongAndSecure12345!",
    "Issuer": "_ISSUER_",
    "Audience": "Loyalty_Members"
  }
}
```

### 4\. Apply Database Migrations

Use the `dotnet-ef` tool to create and apply the database schema.

```bash
# Create the initial migration files based on your DbContext and Models
dotnet ef migrations add InitialCreate

# Apply the migration to your database
dotnet ef database update
```

### 5\. Run the Application

You're all set\! Build and run the project.

```bash
dotnet build && dotnet run
```

The API will be available at `http://localhost:5245/` (or a similar port).

  - **API Base URL:** `http://localhost:5245/`
  - **Swagger UI:** `http://localhost:5245/swagger/index.html`

### 6\. Frontend Setup

Frontend is a static site (HTML + CSS & Vanilla JS) consuming backend APIs.

- **Install dotnet-serve (one-time)**

```bash
dotnet tool install --global dotnet-serve
```

- **Serve the Frontend**

```bash
cd Frontend
dotnet serve -p 5500
```

- **Frontend runs at:** `http://localhost:5500`

-----