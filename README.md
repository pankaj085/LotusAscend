<p align="center">
  <a href="https://github.com/pankaj085/LotusAscend">
    <img src="/DocsAssets/LotusAscend_Logo.svg" alt="LotusAscend" width="350"/>
  </a>
</p>

# LotusLedger - Product Inventory API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-12-4169E1?style=for-the-badge&logo=postgresql)
![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)
![Status](https://img.shields.io/badge/status-active-brightgreen.svg?style=for-the-badge)

</div>
 
 ## 📂 Project Structure

The project follows a clean, logical and most importantly 'Modular' structure to promote separation of concerns.

```plaintext
LotusAscend/
│
├── AppDataContext/           # EF Core DbContext
│   └── AppDbContext.cs
│
├── Contracts/                # DTOs and request/response models
│   ├── AuthDtos.cs
│   ├── CouponDtos.cs
│   └── MemberResponse.cs   
│
├── Controllers/              # API endpoints
│   ├── MemberController.cs
│   ├── PointsController.cs
│   └── CouponController.cs
│
├── Interfaces/               # Abstractions
│   ├── IMemberService.cs
│   ├── IPointsService.cs
│   └── ICouponService.cs
│
├── MappingProfiles/          # AutoMapper config
│   └── MappingProfile.cs
│
├── Middleware/               # Custom middlewares
│   └── ExceptionMiddleware.cs
│
├── Models/                   # EF Core entity models
│   ├── Member.cs
│   ├── OTP.cs
│   ├── PointTransaction.cs
│   └── Coupon.cs
│
├── Services/                 # Business logic
│   ├── MemberService.cs
│   ├── PointsService.cs
│   └── CouponService.cs
│
├── appsettings.json          # Config (DB, JWT)
└── Program.cs                # App startup + DI setup
```

## Install NuGet Packages
This project relies on several NuGet packages. Run the following commands to install them:

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.8
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.8         
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.8
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package FluentValidation.AspNetCore --version 11.3.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.1.2

dotnet tool install --global dotnet-serve # for running the frontend via: dotnet serve -p 5500
```