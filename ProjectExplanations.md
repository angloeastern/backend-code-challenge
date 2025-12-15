# Project Overview: Anglo-Eastern Backend Challenge

This document serves as a comprehensive guide to functionality, architecture, logic, and deployment for the Anglo-Eastern Ship Management System.

## 1. Project Description
This project is a specialized backend solution designed for the maritime industry. It provides a robust system for managing:
*   **Ships**: Managing fleet details, activation status, and fiscal year configurations.
*   **Crew**: Tracking seafarer service history, including Onboard, Relief Due, and Signed Off statuses.
*   **Financials**: Complex budgeting and actual expense reporting with **Fiscal Year-To-Date (YTD)** calculations that vary by ship.
*   **Access Control**: User management with ship-specific assignment logic.

The system is built to be **Enterprise-Ready**, utilizing strict data integrity rules and high-performance database logic.

## 2. Technology Stack
The solution is built on a modern Microsoft stack, ensuring performance, scalability, and maintainability.

| Component | Technology | Description |
| :--- | :--- | :--- |
| **Backend Framework** | **.NET 10.0** | Utilization of the latest .NET preview for maximum performance. |
| **Language** | **C# 13** | Typed, modern, and expressive language features. |
| **Database** | **SQL Server** | Relational database (T-SQL) with heavy usage of Stored Procedures. |
| **ORM** | **EF Core** | Used for object mapping and lightweight data access. |
| **API** | **ASP.NET Core Web API** | RESTful architecture with JWT Authentication and Versioning. |
| **Containerization** | **Docker** | Full `docker-compose` support for instant environment setup. |
| **Testing** | **xUnit** | Unit testing framework for reliable code verification. |
| **Documentation** | **Swagger / OpenAPI** | Interactive API documentation available at runtime. |

## 3. Architecture Design
The project follows **Clean Architecture** principles to separate concerns and ensure the core business logic remains independent of external frameworks.

### Layer Breakdown
1.  **Domain Layer (`AEChallenge.Domain`)**
    *   **Role**: The core. Defines business entities (`Ship`, `CrewMember`, `Budget`) and rules.
    *   **Dependencies**: None. Pure C#.
    
2.  **Application Layer (`AEChallenge.Application`)**
    *   **Role**: Business orchestration. Defines *Use Cases*, interfaces (`IRepositories`), and DTOs.
    *   **Dependencies**: Domain Layer.

3.  **Infrastructure Layer (`AEChallenge.Infrastructure`)**
    *   **Role**: Implementation details. Contains Database Context (`AppDbContext`), Repository implementations, and external service integrations.
    *   **Dependencies**: Application and Domain Layers.

4.  **API Layer (`AEChallenge.Api`)**
    *   **Role**: Entry point. Handles HTTP Requests, Authentication, and Dependency Injection.
    *   **Dependencies**: All other layers.

## 4. Key Business Logic & Features

### 4.1 Ship Management & Fiscal Years
*   **Concept**: Ships are the central unit. A unique feature is the **Fiscal Year** setting (e.g., '0403' for Apr-Mar).
*   **Impact**: This setting is not just metadata; it fundamentally changes how reports are calculated. The system dynamically adjusts "Year-To-Date" sums based on this start month.

### 4.2 Crew Management & Dynamic Status
*   **Challenge**: Crew status changes daily. Storing static status (e.g., "Onboard") in a table is unreliable.
*   **Solution**: The system calculates status **on-the-fly** via Stored Procedures (`SP_GetCrewList`).
    *   **Onboard**: `SignOnDate` < Today < `SignOffDate`.
    *   **Relief Due**: Currently onboard, but `EndOfContractDate` passed >30 days ago.
    *   **Signed Off**: `SignOffDate` is set.

### 4.3 Financial Reporting (The Complex Part)
*   **Chart of Accounts (COA)**: A hierarchical tree (Parent -> Child). Expenses are booked against child accounts but reported at aggregate levels.
*   **YTD Calculation**: The database engine looks up the ship's fiscal year, determines the start month, and sums up actuals/budgets from that start month to the requested period.
*   **Performance**: All this heavy lifting happens in SQL Server, returning a fast, ready-to-render dataset to the API.

## 5. Security Architecture (Authentication & Authorization)
The system employs a **Multi-Layered Security Strategy** to protect data.

### 5.1 Layer 1: API Key Validation (Gateway Security)
Before any logic is executed, the request must pass the API Key check.
*   **Mechanism**: Custom Middleware `ApiKeyMiddleware`.
*   **Requirement**: Every request must have the header `AE_Key`.
*   **Configuration**: The valid key is stored in `appsettings.json` (Secret: `ApiKeys:AE_Key`).
*   **Behavior**: If the key is missing or invalid, the API returns `401 Unauthorized` immediately. *Note: Swagger UI and Root path `/` are whitelisted.*

### 5.2 Layer 2: JWT Authentication (User Identity)
Once the API Key is validated, the system checks *who* is making the request.
*   **Mechanism**: Standard `JwtBearer` Authentication.
*   **Requirement**: Protected endpoints require the header `Authorization: Bearer <valid_jwt_token>`.
*   **Flow**:
    1.  User calls `POST /api/v1/auth/login` with username/password.
    2.  System validates credentials and issues a **JSON Web Token (JWT)** signed with a secret key.
    3.  This token contains the User's Identity (Claims).

### 5.3 Layer 3: Application Authorization (Data Access)
Authentication confirms *who* you are; Authorization determines *what* you can see.
*   **Ship-Level Security**: A user is not automatically entitled to view all ships.
*   **Enforcement**: The API checks the `UserShipAssignment` table.
*   **Scenario**: If User A tries to view Crew Roster for 'SHIP01', the Service layer first validates "Is User A assigned to SHIP01?". If not, it returns `403 Forbidden`.

## 6. How to Run the Application

### Option A: Using Docker (Fastest)
Ideal for testing the full system without local dependency configuration.
1.  Open a terminal in the solution root.
2.  Run: `docker-compose up --build`
3.  Access the API Documentation:
    *   URL: `http://localhost:8080/swagger`

### Option B: Local Development
Ideal for coding and debugging.
1.  **Prerequisites**: SQL Server running locally.
2.  **Database Setup**: Execute the provided SQL scripts in the `Database/` folder in this order:
    1.  `CREATE TABLE`
    2.  `PROGRAMMABILITY` (Functions -> Views -> Stored Procedures)
    3.  `DUMMY DATA`
3.  **Configuration**: Update `appsettings.Development.json` in the API project with your local connection string.
4.  **Run**: Execute `dotnet run` inside the `AEChallenge.Api` folder.

## 7. API Reference
The API is compliant with REST standards.

*   **Auth**: `POST /api/v1/auth/login` - *Get Access Token*
*   **Ships**: `GET /api/v1/ships` - *List Ships*
*   **Crew**: `GET /api/v1/crew/ship/{code}` - *Get Roster*
    *   Params: `page`, `status` (Onboard/Relief Due)
*   **Finance**: `GET /api/v1/finance/report/summary` - *Get Financials*
    *   Params: `shipCode`, `period` (e.g., 202507)

The system is fully documented via Swagger UI, allowing you to test every endpoint directly from the browser.
