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
(Optional) By default, the system will automatically pull the data from http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual API and insert/update the platforms and wells table
```
- Open the project solution in Visual Studio (I'm using 2022)
- Press F5 to start
- Login user via swagger API
	- username: admin
	- password: Test@123
- Get & copy the bearer token returned
- Put the bearer token in the authentication field on top right of the Web API. E.g. Bearer <token>
- Use the /api/platforms/actual to insert/update the table for actual data
- Use the /api/platforms/dummy to insert/update the table for dummy data
- Use the /api/platforms to test the data accordingly
```

### 5. Part 1: Assessment
- Project Setup (.Net Core API + EF Core, SQL, etc.) 0.5 hr
- Create Models and configure EF Code First 0.5 hr
- Configure login to  0.5 hr
- Implement API call and deserialize into DTOs 0.5 hr
- Insert/update into DB 1 hr
- Test for both actual & dummy data 0.5 hr
- Optimization 0.5 hr
- Git setup, updated readme, push 0.5 hr
- SQL Query optimization 1 hr

### 6. Part 2: Assessment
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