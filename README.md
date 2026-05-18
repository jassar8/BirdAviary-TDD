# Bird Aviary Management System

A modern desktop application for managing a bird aviary, built with **C# WPF**, **MVVM architecture**, and **Test-Driven Development (TDD)**.

## Features

| Page | Description |
|------|-------------|
| **Dashboard** | Total birds, sale count, average age, isolation count, activity feed, type distribution chart |
| **Add Bird** | Register birds with validation (Ring ID, type, color, hatch year, status) |
| **Bulk Load** | Generate 10,000 random birds with progress bar and performance timer |
| **Inventory Report** | Sortable DataGrid, search/filter, statistics panel, export UI |
| **TDD Testing** | In-app test runner, pass/fail counters, Moq simulation, sorting benchmarks |

## Architecture

```
BirdAviary-TDD/
├── src/
│   ├── BirdAviary.Core/          # Business logic (no UI dependencies)
│   │   ├── Enums/
│   │   ├── Interfaces/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Sorting/              # Bubble Sort → Merge Sort refactor
│   └── BirdAviary/               # WPF presentation layer
│       ├── Converters/
│       ├── Themes/               # Dark mode UI
│       ├── ViewModels/
│       └── Views/
└── tests/
    └── BirdAviary.Tests/         # NUnit + Moq
```

## Tech Stack

- **.NET 9** — WPF desktop app
- **MVVM** — CommunityToolkit.Mvvm
- **NUnit** — Unit testing framework
- **Moq** — Mocking dependencies in tests
- **Bubble Sort / Merge Sort** — Sorting algorithm comparison for TDD coursework

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Run the Application

```bash
dotnet run --project src/BirdAviary/BirdAviary.csproj
```

### Run Tests

```bash
dotnet test
```

## TDD Workflow

Tests are organized by layer:

- `BirdServiceTests` — Validation, CRUD, dashboard stats (with Moq mocks)
- `BubbleSortServiceTests` — Initial O(n²) sorting implementation
- `MergeSortServiceTests` — Refactored O(n log n) implementation + performance comparison
- `BirdRepositoryTests` — Data persistence

The in-app **TDD Testing** page runs a built-in test suite and benchmarks both sorting algorithms side-by-side.

## UI Design

- Dark mode inspired by Visual Studio / JetBrains Rider
- Glassmorphism cards with soft shadows
- Rounded corners and smooth hover states
- Soft blue accent palette on dark gray backgrounds

## License

University software engineering project — TDD & software quality focus.
