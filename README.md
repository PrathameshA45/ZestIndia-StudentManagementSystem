# ZestIndia-StudentManagementSystem

![.NET](https://img.shields.io/badge/.NET-10.0-blue)
![C#](https://img.shields.io/badge/C%23-10.0-green)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

## 🚀 Project Overview

**Zest India IT - Student Management System** is an ASP.NET Core Web API project designed to manage students. It features:

- **CRUD operations** for students (Create, Read, Update, Delete)
- **JWT Authentication** for secure endpoints
- **CQRS Pattern** with MediatR
- **Swagger API Documentation**
- **Global Exception Handling**
- **Serilog Logging**
- **Layered Architecture** for clean code and maintainability

This project was developed as part of the **Zest India IT Assignment**.

---

## 📁 Project Structure
Structure.Api (Controllers, Entry Point)
│
├── Structure.Data (DTOs, Models)
├── Structure.Domain (DbContext, Entities)
├── Structure.Infrastructure (DI, JWT, Mapping)
├── Structure.MediatR (CQRS: Commands, Queries, Handlers)
└── Structure.Repository (Data Access Layer, Repositories)

---

## 🔐 Features

### Core Features
- Get all students
- Get student by ID
- Add new student
- Update student
- Delete student

### Security & Quality
- JWT Authentication
- Global Exception Handling
- Serilog Logging
- Swagger UI documentation
- Layered architecture (Clean Architecture + Repository + Unit of Work)

---

## ⚡ Quick Start

1. Clone the repository:

```bash
git clone https://github.com/<your-username>/ZestIndia-StudentManagementSystem.git
cd ZestIndia-StudentManagementSystem/Structure.Api
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=StudentDB;Trusted_Connection=True;"
}
Apply migrations:
dotnet ef database update
Run the API:
dotnet run
Open Swagger UI in browser:
https://localhost:5001/swagger
📝 API Endpoints
Method	Endpoint	Description
POST	/api/auth/login	Get JWT token
GET	/api/students	Get all students
GET	/api/students/{id}	Get student by ID
POST	/api/students	Create student
PUT	/api/students/{id}	Update student
DELETE	/api/students/{id}	Delete student
📊 Tech Stack
ASP.NET Core 10.0
Entity Framework Core
MediatR (CQRS)
JWT Authentication
Serilog Logging
Swagger / OpenAPI
SQL Server (or any RDBMS)
✅ Pre-Submission Checklist
 API runs without errors
 All endpoints tested in Swagger UI
 JWT authentication works
 Logging is enabled (Serilog)
 Database persists data
 README and .gitignore included
 GitHub repository ready for submission
📚 Resources
ASP.NET Core Documentation
JWT Authentication
MediatR CQRS Pattern
Entity Framework Core
🏆 Status

Ready for submission ✅
Last Updated: 2026-05-14
Version: 1.0.0


---

### **.gitignore**

This is a standard `.gitignore` for Visual Studio + .NET projects:

```gitignore
# Visual Studio
.vs/
*.user
*.suo
*.userosscache
*.sln.docstates

# Build results
bin/
obj/
out/

# Rider
.idea/

# OS Files
.DS_Store
Thumbs.db

# NuGet Packages
*.nupkg
packages/

# ASP.NET Core
appsettings.Development.json
wwwroot/
logs/
*.log

# EF Core Migrations
Migrations/

# User Secrets
secrets.json

# Resharper
_ReSharper*/