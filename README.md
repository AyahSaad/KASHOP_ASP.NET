# KASHOP ASP.NET Core Web API

**KASHOP** is an ASP.NET Core Web API project designed as part of the **Knowledge Academy ASP.NET course**.  
https://documenter.getpostman.com/view/36257320/2sBXc8q4Bz?authuser=0
---

## 🌟 Key Highlights

- **3-Tier Architecture**:DAL (Data Access Layer), BLL (Business Logic Layer), PL (Presentation Layer)
- **JWT Authentication  & Authorization**:ASP.NET Identity, JWT + Refresh Tokens, Role-Based Access Control (User / Admin / Super Admin),
Claims-Based Authorization, Email confirmation for secure account activation
- **Core E-Commerce Modules**:Products, Categories, Cart, Orders, Reviews, Checkout
- **Payments**:Cash on Delivery, Card payments via Stripe  
- **Data Handling & Performance**:Pagination, Filtering, Search, **Repository Pattern** for clean and maintainable code
- **Localization**:Multi-language support (Arabic / English)
- **Error Handling & Validation**:Global error handling, Custom validation responses
- **Integration & Security**:CORS policy enabled for frontend integration

---

## 🛠️ Technologies & Tools Used

- **ASP.NET Core** – Web API framework for building backend services.
- **ASP.NET Identity** 
- **C#** – Primary programming language.  
- **Entity Framework Core** – ORM for database management.  
- **JWT (JSON Web Tokens)** – Authentication for secure API access.  
- **Role-based & Claims-based Authorization** – Fine-grained access control.  
- **SQL Server / LocalDB** – Database for storing application data.  
- **Swagger / Postman** – API documentation and testing.
- **Mapster** – Object mapping.  
- **Stripe** – Online payments.  

---

## 📂 Project Structure

The project follows a **layered architecture** for better maintainability:

- **Controllers** – Handle HTTP requests and return responses.  
- **Models / DTOs** – Define data structures for API requests and responses.
- **Repositories (DAL) – Database operations**
- **Services / Business Logic** – Contains core application logic.  
- **Data / DbContext** – EF Core database management.
- **Configuration** – App settings, localization, JWT secrets, and authentication settings.  
---

## ⚠️ Project Status

- Completed
- All core features including authentication, authorization, payments, localization, and CRUD operations have been fully implemented.
---
