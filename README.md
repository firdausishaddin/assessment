# AEM Enersol Assessment .NET API

This project is a .NET 8 Web API that fetches and syncs platform and well data from a remote REST API, supporting dynamic structure changes.

## 🔧 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express / LocalDB
- Optional: [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/firdausishaddin/assessment.git
cd assessment
```

### 2. Restore Packages
```
dotnet restore
```

### 3. Add Entity Framework Packages (if not yet installed)
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### 4. Apply Migrations and Update Local Database (if no migrations)
I am using Docker Container in Synology DS923+ server for SQL Server connection since SQL Server does not work on my local machine
Change the connection string in appsettings.json to point to your local SQL database
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Running & Testing
By default, the system will automatically pull data from API and insert/update the Platforms and Wells tables.

🧪 Steps to Test
Open the project solution in Visual Studio 2022 (or your preferred IDE).

Sample `appsettings.json` Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AssessmentDb;User Id=sa;Password=yourStrong(!)Password;"
  }
}
```

Press F5 to start debugging.
- Navigate to the Swagger UI in your browser.
- Authenticate using the following credentials:
```
Username: admin
Password: Test@123
```

After logging in:
- Copy the returned Bearer token.
- Click the Authorize button in Swagger (top-right).
- Paste the token like this: Bearer <your_token_here>.

Test the endpoints:
- 🔄 GET /api/platforms/actual — Sync actual API data into DB.
- 🧪 GET /api/platforms/dummy — Sync dummy API data into DB.
- 📦 GET /api/platforms — Retrieve and verify stored data.

### 6. Part 1: Assessment
| Task                                      | Est. Time |
|-------------------------------------------|-----------|
| Project setup (.NET Core API + EF + SQL)  | 0.5 hr    |
| Models and EF Code-First setup            | 0.5 hr    |
| Login authentication                      | 0.5 hr    |
| API call & DTO deserialization            | 0.5 hr    |
| Insert/update database logic              | 1.0 hr    |
| Test both actual & dummy data             | 0.5 hr    |
| Optimization and refinements              | 0.5 hr    |
| Git setup, push to GitHub                 | 0.5 hr    |
| Beautiful README.md                       | 0.5 hr    |
| SQL query and optimization                | 1.0 hr    |
| **Total**                                 | **5.5 hr**|
💡 _Shoutout to beautiful README.md — deserves a 💕_

### 7. Part 2: Assessment
```
SELECT
	p.UniqueName as PlatformName,
	w.WellId as Id,
	p.PlatformId as PlatformId,   
    w.UniqueName,
    w.Latitude,
    w.Longitude,
    w.CreatedAt,
    w.UpdatedAt
FROM Wells w
INNER JOIN (
    SELECT PlatformId, MAX(UpdatedAt) AS LatestUpdate
    FROM Wells
    GROUP BY PlatformId
) latest ON w.PlatformId = latest.PlatformId AND w.UpdatedAt = latest.LatestUpdate
JOIN Platforms p ON p.PlatformId = w.PlatformId
ORDER BY p.Id;
```