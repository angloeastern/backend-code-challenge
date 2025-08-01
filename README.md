# AE Backend Code Challenge

## Overview

Welcome to the Backend Code Challenge.  
This challenge assesses your ability to design and implement a backend solution that includes:

- Designing a SQL Server database schema and stored procedures  
- Building REST APIs in C# that consume these stored procedures  
- Implementing features around vessel management, crew, and financial reporting  

**You have 14 days to complete the challenge.**

**Tech stack requirements:**  
- Database: SQL Server
- Backend: C#  
- Testing: xUnit  
- Use of stored procedures is mandatory for database interactions.

---

## Domain Model & Business Rules

The challenge models a ship management system with these main entities:

### Vessel

Each vessel has a unique code and a name. Vessels also have geolocation data specifying their current latitude and longitude, and a velocity (in knots) indicating their current speed. Each vessel is assigned a fiscal year code determining its accounting period — for example, `"0112"` means the fiscal year runs from January to December, while `"0403"` means the fiscal year starts in April and ends in March the following year. The status of a vessel indicates whether it is currently active or inactive in service. Only active vessels should be considered in operational queries and reports.

### Crew

Each vessel maintains a roster of crew members. A crew member is identified by a unique ID and has personal details such as name, nationality, birth date, and rank. Ranks follow a standard maritime classification (e.g., Master, Chief Engineer, Chief Officer), and typically only one crew member holds each rank on board during a contract. [^1]

Crew service history is recorded per vessel and includes contract periods represented by key dates: Sign On Date (when the crew member joins the vessel), Sign Off Date (when they leave), and End of Contract Date (the scheduled end of their contract). These dates determine the crew member’s current status relative to the vessel:

- A crew member with a Sign On Date in the past, no Sign Off Date, and an End of Contract Date not yet passed is considered **Onboard**.
- If the Sign On Date is in the future, the crew member is **Planned** to join.
- If a crew member has no Sign Off Date, but the current date is more than 30 days past their End of Contract Date, their status is **Relief Due**.
- If the Sign Off Date is set, the crew member’s service is considered complete (**Signed Off**).

### User and Vessel Assignment

Users of the system represent application users who can be assigned to one or more vessels. Each user has a name and role within the system. Users can view the vessels they are assigned to. The relationship between users and vessels is many-to-many, allowing for flexible assignment scenarios.

### Ports and Location

The system maintains a list of ports, each identified by a unique name and associated geolocation (latitude and longitude). Port data is seeded initially and remains static for the purposes of this challenge. Ports are used to calculate proximity and estimated arrival times from vessels.

### Financial Data and Accounting

Each vessel has associated financial data aligned with its fiscal year settings. The financial data includes budgeted amounts and actual transaction values, organized by accounting periods (monthly) and account numbers from a hierarchical Chart of Accounts (COA). Accounts can be parent (summary) or child (detail) types, forming a tree-like structure. [^2]

Budgets and actuals are recorded for child accounts, and parent account values should be computed by aggregating their child accounts. Budget and actual data may include zero values, always non-negative values, and are stored for specific vessel codes, account numbers, and accounting periods. Year-to-date (YTD) calculations must respect each vessel’s fiscal year settings, summing values from the start of the fiscal year to the selected period.

---

## Sample Data

### Vessels

| Code   | Name             | FiscalYear | Status  |
|--------|------------------|------------|---------|
| SHIP01 | Flying Dutchman   | 0112       | Active  |
| SHIP02 | Thousand Sunny   | 0403       | Active  |

### Crew Members

| CrewMemberId | FirstName | LastName | BirthDate  | Nationality |
|--------------|-----------|----------|------------|-------------|
| CREW001      | Soka      | Philip   | 1980-07-30 | Greek       |
| CREW002      | Masteros  | Philip   | 1980-07-30 | Greek       |

### Crew Service History

| CrewMemberId | Rank           | ShipCode | SignOnDate | SignOffDate | EndOfContractDate |
|--------------|----------------|----------|------------|-------------|-------------------|
| CREW001      | Master         | SHIP01   | 2025-04-05 | NULL        | 2025-07-05        |
| CREW002      | Chief Engineer | SHIP01   | 2025-04-04 | NULL        | 2025-07-04        |

### Chart of Accounts (COA)

- **7000000** OPERATING EXPENSES (Parent)  
  - **7100000** AWARD AND GRANT TO INDIVIDUALS (Child of 7000000)  
    - **7135000** SCHOLARSHIPS (Child of 7100000)  

### Budget Data

| ShipCode | AccountNumber | AccountPeriod | BudgetValue |
|----------|---------------|---------------|-------------|
| SHIP01   | 7135000       | 2025-01       | 0           |
| SHIP01   | 7135000       | 2025-01       | 1000        |
| SHIP01   | 7135000       | 2025-02       | 1200        |
| ...      | ...           | ...           | ...         |
| SHIP01   | 7135000       | 2025-12       | 900         |

### Account Transactions

| ShipCode | AccountNumber | AccountPeriod | ActualValue |
|----------|---------------|---------------|-------------|
| SHIP01   | 7135000       | 2025-01       | 300         |
| SHIP01   | 7135000       | 2025-01       | 0           |
| SHIP01   | 7135000       | 2025-01       | 700         |

---

## Challenge Requirements

### 1. Database Challenge

1. **Design the Database Schema**
   - Create an Entity Relationship Diagram (ERD) based on the provided business context.
   - ⚠️ Use **SQL Server** as the target database platform and write the DDL in **T-SQL**.
   - Identify and define necessary entities, relationships, and data types.
   - Provide DDL (CREATE TABLE) scripts for all necessary entities.
   - Include primary keys, foreign keys, and appropriate constraints (e.g., budget and actual values must not be negative).

2. **DDL Scripts**  
   - Write SQL scripts to create the necessary tables and relationships.
   - Avoid hardcoding assumptions — focus on building a flexible and normalized design.

3. **Insert Sample Data**  
   - Include meaningful sample data covering:
     - At least 5 vessels with varying fiscal years (e.g., Jan–Dec, Apr–Mar).
     - Crew records with realistic service history (e.g., onboard, relief due, sign-off) with **at least 20 crew members per vessel**, including a variety of ranks and overlapping service periods.
     - Rank types and references for crew with **at least 5 distinct crew rank types**.
     - Chart of Accounts with at least 5 parent accounts, each having a minimum of 5 child accounts arranged in a hierarchy of at least 3 levels.
     - Budget data for at least 3 vessels (aligned with the crew data), spanning fiscal years 0112 and 0403, covering the full periods of 2024 and 2025, with values for a minimum of 5 accounts.
     - Account transaction data for at least 3 vessels (corresponding to those in the budget and crew data), covering fiscal years 0112 and 0403, including at least 6 periods in 2025 and the full period of 2024.

4. **Stored Procedures / Views**  
   - Crew List: Create a stored procedure that returns crew members currently onboard or relief due for a selected vessel as of today's date, supporting:
     - Pagination (page size and page number)
     - Sorting by any column (Rank Name, Crew ID, First/Last Name, Age, Nationality, SignOnDate, Status)
     - Searching (filter by any column except Status, including partial date matches like `05 Apr`)
     - Exclude any crew members who have signed off (i.e., crew with a non-null SignOffDate).
    
   - Financial Header: Create stored procedures that return a summary of all accounts for a specific vessel on a year-to-date (YTD) basis according to the vessel's fiscal year, including:
     - Total Budget (full year, e.g., Jan–Dec for the selected year): sum of all budget values for the year.
     - Actual (for the selected period).
     - Budget Actual (for the selected period).
     - Variance Actual (Actual - Budget Actual).
     - Actual YTD (from the start of the fiscal year up to the selected period).
     - Budget YTD (from the start of the fiscal year up to the selected period).
     - Variance YTD (Actual YTD - Budget YTD).
   
   - Financial Reports: Create stored procedures to return detailed and summary financial expense reports for a specific vessel and period, including:
     - COA Description and Account Number
     - Actual and Budget values for the selected period
     - Variances (Actual - Budget)
     - Year-To-Date (YTD) Actuals, Budgets, and Variances calculated based on the vessel’s fiscal year
    
#### Financial Data Notes (Summary)

- The Chart of Accounts (COA) uses a hierarchical account structure with parent (summary) and child (detail) accounts.
- Budgets and actual transaction amounts are recorded at the child account level only.
- Parent account values must be calculated by aggregating their child accounts.
- Fiscal year codes (e.g., "0112" for Jan-Dec, "0403" for Apr-Mar) define the YTD calculation period.
- Stored procedures must calculate monthly and YTD values for budget and actuals, as well as variances.
- Only include records where budget or actual values are non-zero.
    
#### Example Output (Crew List)

| Rank Name      | Crew Member ID | First Name | Last Name  | Age | Nationality | SignOnDate | Status   |
|----------------|----------------|------------|------------|-----|-------------|------------|----------|
| Master         | CREW001        | Soka       | Philip     | 45  | Greek       | 05 Apr 2025| Onboard  |
| Chief Engineer | CREW002        | Masteros   | Philip     | 45  | Greek       | 05 Apr 2025| Onboard  |
| Chief Officer  | CREW003        | John       | Masterbear | 50  | Greek       | 08 Apr 2025| Onboard  |
| Cadet          | CREW004        | Bob        | Marley     | 27  | Mexican     | 05 Apr 2025| Onboard  |
| Oiler          | CREW005        | John       | Chena      | 30  | Mexican     | 08 Apr 2025| Relief Due |

#### Example Output (Financial Header)

| Total Budget (Jan–Dec 2025) | Actual (Jul 2025) | Budget Actual (Jul 2025) | Variance Actual | Actual YTD (Jan–Jul 2025) | Budget YTD (Jan–Jul 2025) | Variance YTD (Jul 2025) |
|-----------------------------|-------------------|--------------------------|-----------------|---------------------------|---------------------------|-------------------------|
| 20100                       | 3300              | 3300                     | 0               | 9600                      | 9900                      | 0                       |

#### Example Output (Financial Expense Detail)

| COA Description            | Account Number | Actual (Jul 2025) | Budget (Jul 2025) | Variance Actual | Actual YTD (Jan-Jul 2025) | Budget YTD (Jan-Jul 2025) | Variance YTD |
|----------------------------|----------------|-------------------|-------------------|-----------------|---------------------------|---------------------------|--------------|
| OPERATING EXPENSE           | 7000000        | 3300              |                   | 3300            | 9600                      | 9900                      | 0            |
| AWARD AND GRANT TO INDIVIDUALS | 7100000        | 3300              | 3300              | 0               | 9600                      | 9900                      | 0            |
| AWARDS                     | 7120000        |                   | 1000              | 1100            | -100                      | 3200                      | 3300         | -100         |
| SCHOLARSHIPS               | 7135000        | 2300              | 2200              | 100             | 6400                      | 6300                      | 100          |

Access all data only via stored procedures. Use realistic data and ensure correctness of fiscal year–based YTD calculations.

---

### 2. API Challenge

#### Objectives:

- Build REST APIs in C# that consume your stored procedures.
- Implement endpoints for users, vessels, crews, and financial reporting.
- Implement the following user stories (including but not limited to):
  - Add and list users.
  - Add and list ships.
  - Assign ships to users and retrieve ships by user.
  - Update ship velocity.
  - Get the closest port to a ship with estimated arrival time.
  - Retrieve crew lists for vessels (per database SP).
  - Retrieve financial reports per vessel and accounting period (per database SP).

#### Requirements

- APIs should interact only with the database via stored procedures.
- APIs should handle errors and edge cases gracefully
- Follow RESTful principles.
- Follow best practices in code structure, naming, and testability.
- Use appropriate HTTP methods and status codes.
- Write unit tests using xUnit.

---

## Evaluation Criteria

Your submission will be evaluated based on the following aspects:

- **Database design:** Appropriate data types, constraints, lengths, and relationships.
- **Stored procedure logic:** Accurate calculations covering all required features and functions.
- **Code quality and readability:** Clean, maintainable, and well-structured code.
- **Testing:** Unit tests or scripts demonstrating correctness.
- **Error and exception handling:** Robust handling of edge cases and failures.
- **Best practices and principles:** Adherence to clean code, modularity, and design patterns.
- **Basic system design:** Overall architecture and scalability considerations.
- **Documentation:** Clear README explaining your design decisions and how to run your solution.

### Nice to have

- Use of scalar or table-valued functions to promote logic reuse.
- Swagger UI for API documentation.
- Docker setup for easy environment setup and deployment.
- Basic CI scripting to automate tests or builds.

---

## Deliverable

Please create a **GitHub repository** containing your full solution, including:

- Database scripts (DDL, stored procedures, sample data)
- API source code and tests
- Documentation (README)
- Any additional files (e.g., Dockerfile, CI scripts, Swagger specs)

Share the GitHub repo link with us via e-mail.

---

If you have any questions or need clarifications, feel free to contact us by e-mail.

[^1]: Reference for crew ranks: [https://nedcon.ro/crew-structure-on-board-merchant-vessels-deck-department/](https://nedcon.ro/crew-structure-on-board-merchant-vessels-deck-department/ )
[^2]: Reference for chart of accounts: [https://www.gocivilairpatrol.com/media/cms/R173_001_CoA_3918FC7AF372F.pdf](https://www.gocivilairpatrol.com/media/cms/R173_001_CoA_3918FC7AF372F.pdf)
