# PayrollAssessment API

A RESTful Payroll System API built with **ASP.NET Core**, **Entity Framework Core**, and **MySQL (XAMPP)**.

---

```
PayrollAssessment/
├── PayrollAssessment.API/            → Controllers, startup config (entry point)
├── PayrollAssessment.Services/       → Business logic & payroll computation
├── PayrollAssessment.Repositories/   → Database queries & EF DbContext
├── PayrollAssessment.DataModels/     → Entity classes (maps to DB tables)
└── PayrollAssessment.BusinessModels/ → Request & Response models (DTOs)
```

---

### Request Flow
```
HTTP Request
     ↓
Controller        → Receives HTTP request, calls Service
     ↓
Service           → Business logic, payroll computation, maps data
     ↓
Repository        → Queries the database via EF Core
     ↓
MySQL Database    → Stores and retrieves employee data
```

### Employee Number Generation
Employee numbers are auto-generated using:
- First **3 letters** of last name
- A **5-digit** random number (padded with zeros)
- Date of birth in **ddMMMyyyy** format

**Example:** `DELA CRUZ` born `May 17, 1994` → `DEL-12340-17MAY1994`

### Take-Home Pay Computation
- Employee works **thrice a week** based on schedule:
  - `MWF` = Monday, Wednesday, Friday
  - `TTHS` = Tuesday, Thursday, Saturday
- Each **work day** = `2x daily rate`
- On **birthday** = `+1x daily rate` (stacks on top of work day if applicable)

**Example:**
```
Employee : SY, ANNIE  
Daily Rate: 1,500  
Schedule  : TTHS  
Birthday  : September 1  
Period    : Sep 1–9, 2011

Sep 1 (Thu) = Work day (3,000) + Birthday bonus (1,500) = 4,500
Sep 3 (Sat) = Work day = 3,000
Sep 6 (Tue) = Work day = 3,000
Sep 8 (Thu) = Work day = 3,000
─────────────────────────────
Total Take-Home Pay = PhP 13,500.00 
```

---

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core 9 |
| Database | MySQL via XAMPP |
| MySQL Driver | Pomelo.EntityFrameworkCore.MySql 9.0.0 |
| API Docs | Swagger (Swashbuckle) |

---

### Prerequisites
- Visual Studio 2022/2026
- XAMPP (MySQL running)
- .NET 9 SDK

### Setup
1. Clone the repository
```bash
git clone https://github.com/YOUR_USERNAME/PayrollAssessment.git
```

2. Create the database in phpMyAdmin (`http://localhost/phpmyadmin`)
```sql
CREATE DATABASE payrollassesmentdb;

USE payrollassesmentdb;

CREATE TABLE employees (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EmployeeNumber VARCHAR(50) NOT NULL UNIQUE,
    LastName VARCHAR(100) NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    MiddleName VARCHAR(100),
    DateOfBirth DATE NOT NULL,
    DailyRate DECIMAL(10,2) NOT NULL,
    WorkingDays VARCHAR(10) NOT NULL
);
```

3. Update `appsettings.json` in `PayrollAssessment.API`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=payrollassesmentdb;User=root;Password=;"
  }
}
```

4. Run the project — Swagger will open at `https://localhost:PORT/swagger`

---


| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Employee/list` | Get all employees |
| GET | `/api/Employee/{employeeNumber}` | Get employee by number |
| POST | `/api/Employee` | Create new employee |
| PUT | `/api/Employee/update/{employeeNumber}` | Update employee |
| DELETE | `/api/Employee/delete/{employeeNumber}` | Delete employee |
| POST | `/api/Employee/compute` | Compute take-home pay |

---


### 1. Add Employee (POST `/api/Employee`)
```json
{
  "lastName": "DELA CRUZ",
  "firstName": "JUAN",
  "middleName": "",
  "dateOfBirth": "1994-05-17",
  "dailyRate": 2000.00,
  "workingDays": "MWF"
}
```
**Response:**
```json
{
  "id": 1,
  "employeeNumber": "DEL-12340-17MAY1994",
  "employeeName": "DELA CRUZ, JUAN",
  "dateOfBirth": "1994-05-17",
  "dailyRate": 2000.00,
  "workingDays": "MWF"
}
```

---

### 2. Get All Employees (GET `/api/Employee/list`)
```json
[
  {
    "id": 1,
    "employeeNumber": "DEL-12340-17MAY1994",
    "employeeName": "DELA CRUZ, JUAN",
    "dateOfBirth": "1994-05-17",
    "dailyRate": 2000.00,
    "workingDays": "MWF"
  }
]
```

---

### 3. Get Employee by Number (GET `/api/Employee/DEL-12340-17MAY1994`)
```json
{
  "id": 1,
  "employeeNumber": "DEL-12340-17MAY1994",
  "employeeName": "DELA CRUZ, JUAN",
  "dateOfBirth": "1994-05-17",
  "dailyRate": 2000.00,
  "workingDays": "MWF"
}
```

---

### 4. Update Employee (PUT `/api/Employee/update/DEL-12340-17MAY1994`)
```json
{
  "lastName": "DELA CRUZ",
  "firstName": "JUAN",
  "middleName": "SANTOS",
  "dateOfBirth": "1994-05-17",
  "dailyRate": 2500.00,
  "workingDays": "MWF"
}
```

---

### 5. Delete Employee (DELETE `/api/Employee/delete/DEL-12340-17MAY1994`)
```
204 No Content ✅
```

---

### 6. Compute Take-Home Pay (POST `/api/Employee/compute`)
```json
{
  "employeeNumber": "SYX-00779-01SEP1994",
  "startingDate": "2011-09-01",
  "endingDate": "2011-09-09"
}
```
**Response:**
```json
{
  "employeeNumber": "SYX-00779-01SEP1994",
  "employeeName": "SY, ANNIE",
  "startingDate": "2011-09-01",
  "endingDate": "2011-09-09",
  "takeHomePay": 13500.00
}
```

---

## Code Design Highlights

- **Encapsulation** — Response models use `private readonly` fields with read-only properties. Data is set once in the constructor and cannot be modified externally.
- **Layered Architecture** — Each layer has a single responsibility and only communicates with the layer directly below it.
- **Extension Methods** — Mapping and computation logic is organized into clean extension methods (`MapToResponse`, `MapToTakeHomePayResponse`, `GenerateEmployeeNumber`).
- **Dependency Injection** — Services and repositories are registered in `Program.cs` and injected via constructors.
