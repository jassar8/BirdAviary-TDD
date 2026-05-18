# Bird Aviary Management System

Professional C# WPF desktop application for bird aviary management, built with **MVVM**, **TDD**, **NUnit**, and **Moq**.

## Solution structure

```
BirdAviary-TDD/
├── src/
│   ├── BirdAviary.Core/
│   │   ├── Enums/
│   │   ├── Helpers/          # ValidationHelper
│   │   ├── Interfaces/       # IBirdService, IHealthService, ISortingService, …
│   │   ├── Models/
│   │   ├── Services/         # BirdService, HealthService, BirdRepository, …
│   │   └── Sorting/          # MergeSortService (+ commented Bubble Sort), BubbleSortService
│   └── BirdAviary/
│       ├── Converters/
│       ├── Helpers/
│       ├── Services/         # AppServices (composition root)
│       ├── Themes/
│       ├── ViewModels/
│       └── Views/
└── tests/
    └── BirdAviaryManagement.Tests/   # NUnit + Moq
```

## Features

| Page | Capabilities |
|------|----------------|
| **Dashboard** | Total birds, for sale, average age, isolation count, activity feed, type chart |
| **Add Bird** | Unique ring ID, 5+ types, English/Hebrew color letters only, hatch year, status (In Aviary / Sold / Isolation), health-gated sale |
| **Bulk Load** | 10,000 birds, progress bar, execution timer |
| **Inventory** | Merge-sorted by hatch year (desc), search/filter, stats panel, modern DataGrid |
| **TDD Testing** | In-app runner, Moq health simulation, Bubble vs Merge benchmark |

## TDD & architecture highlights

- **IHealthService** — `IsBirdHealthy(ringId)`; real impl is random; Moq tests verify sale only when approved
- **Sorting** — Custom **Merge Sort** (in-place, no `OrderBy`/`Sort`); legacy **Bubble Sort** kept as comments in `MergeSortService.cs` and as `BubbleSortService` for benchmarks
- **10,000 records** — Bulk load + sort performance test asserts completion **under 9 seconds**
- **Validation** — Ring ID uniqueness, hatch year range, color letters only (`\p{L}` + Hebrew)

## Run

**From source:**
```bash
dotnet run --project src/BirdAviary/BirdAviary.csproj
dotnet test
```

**Published EXE (for submission/demo):**
```bash
dotnet publish src/BirdAviary/BirdAviary.csproj -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true -o publish
```
Then double-click `publish/BirdAviary.exe` (requires .NET 9 runtime on the machine).

## Test project: BirdAviaryManagement.Tests

All tests are in one teacher-friendly file: `tests/BirdAviaryManagement.Tests/BirdAviarySystemTests.cs`

| Test | Coverage |
|------|----------|
| Add bird successfully | ✓ |
| Reject duplicate ring ID | ✓ |
| Reject invalid hatch year | ✓ |
| Reject invalid color text | ✓ |
| Average age calculation | ✓ |
| Sort returns / preserves count / descending order | ✓ |
| Bulk load 10,000 birds | ✓ |
| Health service approved / denied (Moq) | ✓ |

## Tech stack

.NET 9 · WPF · CommunityToolkit.Mvvm · NUnit · Moq
