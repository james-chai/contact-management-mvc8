﻿# Contact Management MVC8 (.NET 8)

## 🛠 Development Setup

### Prerequisites

Ensure the following tools/SDKs are installed:

1.  **.NET SDK 8.0.314**  
    [Download from Microsoft](https://dotnet.microsoft.com/download/dotnet/8.0)

    **NOTE**: You need the **SDK** (not just the Runtime)
    
    Verify installation with:    
        dotnet --list-sdks

2.  **Visual Studio 2022 (Version 17.9 or later)**
    [Download from Microsoft](https://visualstudio.microsoft.com/downloads/)

    Required workloads:

      - ASP.NET and web development
      - .NET desktop development (for .NET Framework 4.8 support)
    
    Recommended components:
    
      - .NET 8.0 SDK
      - .NET Framework 4.8 targeting pack
      - Git for Windows

3.  **Entity Framework Core CLI Tools**

    Verify Installation
              dotnet ef --version

    Install/update globally:

        dotnet tool install --global dotnet-ef # First-time installation
        dotnet tool update --global dotnet-ef # Update existing installation

4.  Connection String Setup

    Option1: add to User Secrets from Visual Studio 2022 (Recommended for Development, User Secrets only work in .NET Core / .NET 5+ projects)

    - Right-click ContactManagement.Web project in Solution Explorer (must be a .NET Core or .NET 8 project)
    - Select "Manage User Secrets"
    - This opens a secrets.json file. Add your connection string like this:

            {
                "ConnectionStrings:ContactManagementDb": "Server=localhost;Database=ContactManagement_MVC8;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
            }

    Option2: add to User Secrets using dotnet CLI

        dotnet user-secrets init --project ContactManagement.Web
        dotnet user-secrets set "ConnectionStrings:ContactManagementDb" "Server=localhost;Database=ContactManagement_MVC8;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"

    Option 3: add to appsettings.json/appsettings.Development.json of the ContactManagement.Web project

          "ConnectionStrings": {
        	"ContactManagementDb": "<set from User Secrets>"
          }

#### How to run ContactManagement_MVC8 application

  1. Open the ContactManagement_MVC8 solution in Visual Studio 2022.
  2. Right-click the solution in the Solution Explorer and select Clean Solution.
  3. Right-click the solution in the Solution Explorer and select Restore NuGet Packages.
  4. Update the connection string in User Secrets or appsettings.Development.json.
  5. Set ContactManagement.Web as the startup project.
  6. Launch the application.

##### Solution Structure

```
ContactManagement.MVC8/
├── ContactManagement_MVC8.sln
│
├── ContactManagement.Web/           (ASP.NET Core MVC 8 Web Project)
│   ├── Controllers/
│   ├── Views/
│   ├── Models/
│   ├── Program.cs
│   ├── appsettings.json
│   │   └── appsettings.Development.json
│   └── Properties/
│       └── launchSettings.json
│
├── ContactManagement.Core/          (.NET 8 Class Library)
│   └── Models/
│       └── Contact.cs
│
└── ContactManagement.Data/          (.NET 8 Class Library)
    ├── ContactDbContext.cs
    ├── IContactData.cs
    ├── SqlContactData.cs
    ├── DesignTimeDbContextFactory.cs
    └── Migrations/
```

###### EF Core 8 Database Migrations

**NOTE**: Do not use an existing database.

      The project uses EF Code-First, so you don't need to create a database manually.
      The database will be created automatically the first time the application is launched.

      Alternatively, you can create an empty database if you prefer.

However, if you prefer to apply migrations and create the database manually, follow these steps:

  1. Set ContactManagement.Web as the Startup Project in Visual Studio 2022
  2. Ensure the connection string is configured correctly. Refer to ### Prerequisites 3 for setup instructions.
  3. Open the Command Prompt and navigate to the root folder of the solution (/ContactManagement.Web)

Then run the following commands:

    dotnet ef migrations add InitialCreate --project ContactManagement.Data --startup-project ContactManagement.Web
    dotnet ef database update --project ContactManagement.Data --startup-project ContactManagement.Web

If you encounter issues running 'dotnet ef database update', try the following command instead. Make sure to replace the connection string with your own

    dotnet ef database update --project ContactManagement.Data --startup-project ContactManagement.Web --connection "Server=localhost;Database=ContactManagement_MVC8;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
