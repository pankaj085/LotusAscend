<p align="center">
  <a href="https://github.com/pankaj085/LotusAscend">
    <img src="/assets/LotusAscend_Logo.svg" alt="LotusLedger" width="350"/>
  </a>
</p>
 
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
```