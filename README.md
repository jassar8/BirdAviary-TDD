# Bird Aviary Management  
## מערכת לניהול בית גידול לציפורים

A C# Avalonia desktop application for managing birds in an aviary.  
המערכת פותחה כחלק ממטלת TDD, עם דגש על איכות קוד, בדיקות יחידה, מיון יעיל, Mocking, וממשק משתמש נוח.

---

# How to Run on Windows / איך להריץ ב־Windows

## 1. Install .NET SDK

Make sure .NET SDK is installed on the Windows machine.

Check installation:

```powershell
dotnet --version
```

If `dotnet` is not recognized, install the .NET SDK first.

---

## 2. Open the Project Folder

Open PowerShell or CMD inside the main project folder:

```powershell
cd Desktop\BirdAviaryManagement
```

The folder should contain:

```text
BirdAviaryManagement.slnx
BirdAviaryManagement.App
BirdAviaryManagement.Core
BirdAviaryManagement.Tests
README.md
TDD_NOTES.md
```

---

## 3. Restore, Build, and Test

Run:

```powershell
dotnet restore
dotnet build
dotnet test
```

Expected result:

```text
Build succeeded
All tests passed
```

---

## 4. Run the Avalonia Application

```powershell
dotnet run --project BirdAviaryManagement.App
```

If needed, run directly from the project file:

```powershell
dotnet run --project BirdAviaryManagement.App\BirdAviaryManagement.App.csproj
```

---

# How to Run on Mac / איך להריץ ב־Mac

## 1. Install .NET SDK

Install the **.NET 9 SDK** (or newer) on your Mac.

Download: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

Check installation in Terminal:

```bash
dotnet --version
```

You should see `9.x.x` or higher. If `dotnet` is not found, install the SDK and open a new Terminal window.

---

## 2. Get the Project

Clone from GitHub or open the folder you already downloaded:

```bash
git clone https://github.com/faresnbary/BirdAviaryManagement.git
cd BirdAviaryManagement
```

If you already have the repo, go to its folder instead, for example:

```bash
cd ~/Desktop/newpro/BirdAviaryManagement
```

The folder should contain:

```text
BirdAviaryManagement.slnx
BirdAviaryManagement.App
BirdAviaryManagement.Core
BirdAviaryManagement.Tests
README.md
TDD_NOTES.md
```

---

## 3. Restore, Build, and Test

From the project root, run:

```bash
dotnet restore
dotnet build
dotnet test
```

Expected result:

```text
Build succeeded
All tests passed
```

---

## 4. Run the Avalonia Application (recommended)

Start the app from the project root:

```bash
dotnet run --project BirdAviaryManagement.App
```

Or run directly from the project file:

```bash
dotnet run --project BirdAviaryManagement.App/BirdAviaryManagement.App.csproj
```

The Bird Aviary Management window should open. Use **Ctrl+C** in Terminal to stop the app when you are done.

This is the most reliable way to run on Mac (no Gatekeeper issues).

---

## 5. Build a macOS `.app` (optional)

From the project root on a Mac:

```bash
chmod +x publish-mac.sh
./publish-mac.sh
open publish-mac/BirdAviaryManagement.app
```

The script detects Apple Silicon (`osx-arm64`) vs Intel (`osx-x64`), creates a real **`BirdAviaryManagement.app`** bundle, and ad-hoc signs it.

---

## 6. Do not use these on Mac

| Item | Why it fails |
|------|----------------|
| `publish/BirdAviaryManagement.exe` | Windows-only |
| `BirdAviaryManagement.App` folder | Source code project, **not** an application |
| `BirdAviaryManagement.App.app` | Often a renamed/zipped project folder from Safari — macOS shows **"damaged"** / **פגום** |

Always use **`dotnet run`** (section 4) or **`./publish-mac.sh`** (section 5).

---

## 7. Fix: "App is damaged" / פגום ולא ניתן לפתוח

If macOS says the app is **damaged** and recommends the Trash, the app is usually **not** broken. Safari (or a ZIP download) added a quarantine flag because the app is unsigned.

**Step 1 — Use the correct app**

Open only:

```text
publish-mac/BirdAviaryManagement.app
```

after running `./publish-mac.sh`, **not** a folder named `BirdAviaryManagement.App`.

**Step 2 — Remove quarantine**

In Terminal (change the path if needed):

```bash
xattr -dr com.apple.quarantine ~/Downloads/BirdAviaryManagement/publish-mac/BirdAviaryManagement.app
```

Or clear all extended attributes:

```bash
xattr -cr ~/Downloads/BirdAviaryManagement/publish-mac/BirdAviaryManagement.app
```

**Step 3 — First launch**

Right-click the app → **Open** → **Open** (confirm once).  
Or run: `open publish-mac/BirdAviaryManagement.app`

**Step 4 — Still blocked?**

Clone with git instead of downloading a ZIP from the browser:

```bash
git clone https://github.com/faresnbary/BirdAviaryManagement.git
cd BirdAviaryManagement
dotnet run --project BirdAviaryManagement.App
```

---

# Project Overview / סקירת הפרויקט

Bird Aviary Management is a desktop application for managing birds in an aviary.

The system allows:

- Manual bird entry  
- Bulk loading of 10,000 random birds  
- Inventory report generation  
- Sorting birds by hatch year  
- Average age calculation  
- Counting birds available for sale  
- Checking health approval before sale  
- Unit testing with TDD  
- Mocking an external health service  

המערכת מאפשרת ניהול רשומות של ציפורים, הוספה ידנית, יצירת נתונים אוטומטית, הפקת דוח מלאי, בדיקת כשירות למכירה, ובדיקות יחידה מלאות.

---

# Technologies / טכנולוגיות

- C#
- .NET
- Avalonia UI
- MSTest
- Moq
- TDD - Test Driven Development

---

# Project Structure / מבנה הפרויקט

```text
BirdAviaryManagement
│
├── BirdAviaryManagement.App
│   └── Avalonia UI
│
├── BirdAviaryManagement.Core
│   ├── Models
│   └── Services
│
├── BirdAviaryManagement.Tests
│   └── Unit Tests
│
├── README.md
└── TDD_NOTES.md
```

---

# Main Features / יכולות מרכזיות

## Manual Bird Entry / הוספת ציפור ידנית

Each bird contains:

- Ring ID - unique identifier  
- Bird Type  
- Color / Mutation  
- Hatch Year  
- Status  
- Available For Sale  
- Bulk Generated flag  

כל ציפור כוללת מזהה טבעת ייחודי, סוג, צבע/מוטציה, שנת בקיעה, סטטוס, זמינות למכירה, וסימון האם נוצרה אוטומטית.

---

## Bird Status / סטטוס הציפור

The system supports three statuses:

```text
InAviary     = בבית הגידול
Sold         = נמכרה
Quarantine   = בבידוד
```

These statuses are implemented using an enum:

```csharp
public enum BirdStatus
{
    InAviary,
    Sold,
    Quarantine
}
```

---

## Available For Sale / זמינה למכירה

The field `IsAvailableForSale` represents whether the bird is currently available for sale.

A bird can become available for sale only after passing the health check.

ציפור יכולה להיות זמינה למכירה רק אם בדיקת הבריאות מאשרת זאת.

---

# Validation / בדיקות תקינות קלט

The system protects against invalid input.

It handles:

- Empty Ring ID  
- Ring ID with letters or symbols (numbers only)  
- Duplicate Ring ID  
- Invalid Color / Mutation  
- Numbers or symbols inside Color / Mutation  
- Invalid Hatch Year  
- Future Hatch Year  
- Text instead of year  
- Missing bird during health check  

המערכת לא קורסת במקרה של קלט לא תקין, ומציגה הודעות ברורות למשתמש.

---

# Bulk Load / יצירת 10,000 רשומות

The system can generate 10,000 random bird records.

Each generated bird includes valid random data:

- Unique Ring ID  
- Bird Type  
- Color / Mutation  
- Hatch Year  
- Status  
- Available For Sale value  

Bulk generated birds are marked with:

```csharp
IsBulkGenerated = true
```

This allows clearing only generated records without deleting manually added birds.

---

# Clear Bulk / מחיקת רשומות Bulk בלבד

The application includes a Clear Bulk option.

This removes only birds created by the automatic bulk generator:

```csharp
IsBulkGenerated == true
```

Manual birds are not deleted, even if their Ring ID looks similar to bulk IDs. Bulk birds use numeric IDs starting at `900000001`.

This is safer than deleting by Ring ID prefix.

---

# Inventory Report / דוח מלאי

The report includes:

- Total number of birds  
- Average bird age  
- Number of birds available for sale  
- Sorted bird list by hatch year descending  

הדוח מציג סיכום מלאי וממיין את הציפורים לפי שנת בקיעה מהחדשה לישנה.

---

# Sorting / מיון

The assignment required implementing sorting without built-in sort functions.

## First version

Bubble Sort was implemented first:

```text
Bubble Sort - O(n²)
```

## Refactoring

The active sorting algorithm was later improved to:

```text
MergeSort - O(n log n)
```

The Bubble Sort version was kept as a comment inside `BirdSorter.cs`, as required.

---

# Performance / ביצועים

The assignment required sorting 10,000 records in under 9 seconds.

The system includes a performance test.

Measured result:

```text
Sorting 10,000 birds took: 14 ms
```

This is far below the 9000 ms limit.

---

# Mocking / שימוש ב־Mocking

The project includes an external health service simulation.

Interface:

```csharp
public interface IHealthService
{
    bool IsBirdHealthyForSale(string ringId);
}
```

The real `HealthService` returns a random true or false value.

In unit tests, Moq is used to control the result.

Example:

```csharp
healthServiceMock
    .Setup(service => service.IsBirdHealthyForSale("1001"))
    .Returns(true);
```

This allows testing sale approval without relying on random behavior.

---

# Health Check Logic / לוגיקת בדיקת בריאות

The Check Health For Sale action uses:

1. Ring ID  
2. Bird Status  
3. HealthService  

Logic:

```text
If bird does not exist:
    show message

If bird is Sold:
    cannot be approved for sale

If bird is Quarantine:
    cannot be approved for sale

If bird is InAviary:
    call HealthService
```

Only birds with status `InAviary` are checked by the health service.

---

# TDD Process / תהליך TDD

The project follows the Red-Green-Refactor cycle.

## Red

Tests were written before implementation.

Examples:

- Bird model tests  
- BirdService validation tests  
- ReportService tests  
- Sorting tests  
- Bulk generator tests  
- HealthService mocking tests  

## Green

The simplest code was written to make tests pass.

## Refactor

The code was improved while keeping all tests green.

Examples:

- Sorting improved from Bubble Sort to MergeSort  
- Validation methods were separated  
- HealthService was injected through an interface  
- UI was improved without changing business logic  

---

# Unit Tests / בדיקות יחידה

The test project includes:

```text
BirdTests.cs
BirdServiceTests.cs
BirdSorterTests.cs
BulkBirdGeneratorTests.cs
HealthServiceMockTests.cs
ReportServiceTests.cs
```

The system currently passes:

```text
40 / 40 tests
```

---

# UI / ממשק משתמש

The UI is implemented with Avalonia and supports Mac and Windows.

The interface includes:

- Manual bird form  
- Bird type selection  
- Status selection  
- Available for sale checkbox  
- Add Bird button  
- Load Bulk 10,000 button  
- Clear Bulk button  
- Check Health For Sale button  
- Generate Report button  
- DataGrid inventory table  
- Modern popup messages  

The popup messages support:

- Success  
- Error  
- Not Approved / Warning  

---

# Important Notes / הערות חשובות

- The project is written in C# only.  
- The UI is built with Avalonia.  
- The logic is separated into the Core project.  
- Tests are placed in a separate Tests project.  
- Mocking is implemented using Moq.  
- Bubble Sort was not deleted and remains as a comment.  
- MergeSort is the active sorting algorithm.  
- Clear Bulk removes only `IsBulkGenerated == true`.  
- Manual records are protected from accidental deletion.  

---

# Final Verification / בדיקה סופית לפני הגשה

Run:

```bash
dotnet clean
dotnet restore
dotnet build
dotnet test
dotnet run --project BirdAviaryManagement.App
```

Expected result:

```text
Build succeeded
All tests passed
Application launches successfully
```

---

# Submission / הגשה

Recommended submission files:

```text
BirdAviaryManagement.App
BirdAviaryManagement.Core
BirdAviaryManagement.Tests
BirdAviaryManagement.slnx
README.md
TDD_NOTES.md
.gitignore
```

Do not submit unnecessary build folders:

```text
bin
obj
```