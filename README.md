# Paws Home - Pet Adoption & Care Portal (ASP.NET Core MVC + MySQL)

Mini project for **Web Technologies with .NET**.

**Problem:** Animal shelters often manage pets, adoption requests and vaccination records on paper or WhatsApp, so pets get double-booked, requests are lost and vaccine dates are missed.
**Solution:** A web portal where visitors browse pets and apply online, and shelter staff manage pets, care records and applications.

## Features

**For visitors (no login needed)**
- Browse available pets, filter by species, search by name or breed
- View a pet's details and its vaccination & care records
- Add pets to a **shortlist** (kept in the Session)
- Submit an **adoption application** (validated form)
- **Track** application status using their email

**For shelter staff (login: `admin` / `admin123`)**
- Add, edit, delete pets (full CRUD)
- Add, edit, delete vaccination / care records for each pet (full CRUD)
- Review adoption applications: Approve or Reject
  - Approving marks the pet as **Adopted** and auto-rejects the other pending requests for that pet
- Overdue / due-soon vaccine warnings

## Assignment requirement checklist

| Requirement | Where it is in this project |
|---|---|
| MVC architecture | `Models/`, `Views/`, `Controllers/` folders |
| Entity Framework Core + MySQL | `Data/AppDbContext.cs`, `Program.cs` (Pomelo MySQL provider) |
| CRUD operations | Pets and Vaccination Records (Create, Read, Update, Delete); Applications (Create, Read, Update status) |
| Form handling | Pet form, application form, care record form, login, search/filter, track-by-email |
| Model validation | Data annotations in `Models/` (required, email, 10-digit phone, min length, URL, ranges) + extra checks in controllers (duplicate application, due date after given date) |
| State management | **Session**: staff login (`AccountController`, `Filters/RequireLoginAttribute.cs`) and visitor shortlist (`ShortlistController`, `Helpers/SessionExtensions.cs`); **TempData** for alert messages |
| Responsive UI | Bootstrap 5 grid and navbar (`Views/Shared/_Layout.cshtml`) |

## Folder structure

```
PetAdoptionPortal/
├── PetAdoptionPortal.sln                 <- double-click this to open in Visual Studio
├── README.md                             <- this file
├── PROJECT_EXPLAINED.md                  <- simple explanation of every file + viva questions
├── .gitignore
├── Database/
│   └── pet_adoption_db.sql               <- optional schema reference
└── PetAdoptionPortal/
    ├── PetAdoptionPortal.csproj          <- project file + NuGet package list
    ├── Program.cs                        <- app start-up (MVC, database, session)
    ├── appsettings.json                  <- DATABASE CONNECTION STRING (edit this!)
    ├── Properties/launchSettings.json
    ├── Controllers/
    │   ├── HomeController.cs             <- home page
    │   ├── AccountController.cs          <- staff login / logout
    │   ├── PetsController.cs             <- browse + CRUD for pets
    │   ├── VaccinationsController.cs     <- CRUD for care records (staff)
    │   ├── ApplicationsController.cs     <- apply, track, approve / reject
    │   └── ShortlistController.cs        <- favourites stored in Session
    ├── Models/
    │   ├── Pet.cs
    │   ├── VaccinationRecord.cs
    │   ├── AdoptionApplication.cs
    │   ├── PetOptions.cs                 <- species list + status constants
    │   ├── LoginViewModel.cs
    │   └── HomeViewModel.cs
    ├── Data/
    │   ├── AppDbContext.cs               <- connects C# classes to MySQL tables
    │   └── DbSeeder.cs                   <- sample pets
    ├── Filters/
    │   └── RequireLoginAttribute.cs      <- blocks staff pages if not logged in
    ├── Helpers/
    │   └── SessionExtensions.cs          <- save/read lists in Session
    ├── Views/
    │   ├── _ViewImports.cshtml, _ViewStart.cshtml
    │   ├── Shared/ (_Layout, _PetCard, _ValidationScriptsPartial)
    │   ├── Home/ (Index, Error)
    │   ├── Account/ (Login)
    │   ├── Pets/ (Index, Details, Create, Edit, Delete)
    │   ├── Vaccinations/ (Create, Edit)
    │   ├── Applications/ (Apply, Track, Index)
    │   └── Shortlist/ (Index)
    └── wwwroot/
        ├── css/site.css
        └── js/site.js
```

## How to set it up and run it (Visual Studio 2026 or later, Windows)

### Step 1 - Install the tools (one time)

1. **Visual Studio 2022** with the workload **"ASP.NET and web development"**.
2. **.NET 10 SDK** (https://dotnet.microsoft.com/download/dotnet/10.0). Visual Studio may already include it.
3. **MySQL Server 8** using the MySQL Installer (https://dev.mysql.com/downloads/installer/). Set a **root password** during installation and remember it.
   - Optional: **MySQL Workbench** lets you see your tables.

### Step 2 - Put your MySQL password in the project

Open `PetAdoptionPortal/appsettings.json` and replace `YOUR_MYSQL_PASSWORD`:

```json
"DefaultConnection": "Server=localhost;Port=3306;Database=pet_adoption_db;User=root;Password=YOUR_MYSQL_PASSWORD;"
```

You do **not** need to create the database yourself. On the first run the app creates `pet_adoption_db`, all tables and sample pets.

### Step 3 - Run

1. Double-click `PetAdoptionPortal.sln`.
2. Wait for NuGet packages to restore (status bar at the bottom).
3. Press **F5** (or click the green **https** button).
4. The browser opens on the home page. Click **Staff Login** and use `admin` / `admin123`.

### Quick test to see everything working

1. Open **Find a Pet**, click the heart on a pet, check the **Shortlist** counter in the menu.
2. Open Bruno, click **Adopt Bruno**, submit the form (try wrong phone number first to see validation).
3. Click **Track Application** and enter your email.
4. Log in as staff, open **Applications**, **Approve** it, and see Bruno marked as Adopted.
5. Open any pet as staff and add a vaccination record.

## Common problems

| Problem | Fix |
|---|---|
| `Access denied for user 'root'` | Wrong password in `appsettings.json` |
| `Unable to connect to any of the specified MySQL hosts` | MySQL is not running. Press Win+R, type `services.msc`, start **MySQL80** |
| NuGet package not found | Right-click the solution, **Restore NuGet Packages**; check internet |
| `.NET 10 runtime not found` | Install the .NET 10 SDK (Step 1) |
| Styles look broken | Bootstrap loads from the internet (CDN). Connect to the internet |
| Shortlist is empty after closing the browser | Normal: Session data lasts only for the browsing session |
| Want to reset all data | In MySQL Workbench run `DROP DATABASE pet_adoption_db;` and start the app again |


## Upload to GitHub

1. Create a new empty repository on github.com (for example `PetAdoptionPortal`).
2. In the folder that contains the `.sln`, run:
   ```
   git init
   git add .
   git commit -m "Pet Adoption and Care Portal"
   git branch -M main
   git remote add origin https://github.com/<your-username>/PetAdoptionPortal.git
   git push -u origin main
   ```
3. **Before pushing**, put `YOUR_MYSQL_PASSWORD` back in `appsettings.json` so your real password is not public.
4. Submit the repository link on the batch-wise link given by your faculty.
