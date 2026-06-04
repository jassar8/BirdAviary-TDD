# Bird Aviary Management

## Description

Bird Aviary Management is a C# Avalonia application for managing birds in an aviary.

The project was developed using Test Driven Development (TDD), according to the assignment requirements.

The system supports manual bird entry, bulk loading of 10,000 random birds, inventory report generation, sorting by hatch year, average age calculation, and mocking an external health service.

---

## Main Features

- Add birds manually
- Validate bird data
- Prevent duplicate ring IDs
- Choose bird type from a predefined list
- Support at least 5 bird types
- Enter color or mutation using letters only
- Enter hatch year
- Set bird status
- Mark bird as available for sale
- Generate 10,000 random bird records
- Generate inventory report
- Sort birds by hatch year in descending order
- Calculate average bird age
- Count birds available for sale
- Use Mocking for an external health service
- Basic cross-platform UI using Avalonia

---

## Technologies

- C#
- .NET
- Avalonia UI
- MSTest
- Moq

---

## Project Structure

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
└── BirdAviaryManagement.Tests
    └── Unit Tests
```

---

## How to Run

Open a terminal in the main project folder:

```bash
cd BirdAviaryManagement
```

Restore packages:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run the tests:

```bash
dotnet test
```

Run the Avalonia application:

```bash
dotnet run --project BirdAviaryManagement.App
```

---

## TDD Process

The project follows the Red-Green-Refactor cycle.

### Red

Tests were written before the implementation.

Examples:

- Bird model tests were written before creating the Bird model.
- BirdService tests were written before implementing validation logic.
- ReportService tests were written before implementing report calculations.
- Sorting tests were written before implementing the sorting algorithm.
- Mocking tests were written before integrating the health service.

### Green

The simplest code was written to make the tests pass.

Examples:

- Bird, BirdType and BirdStatus were added.
- BirdService was implemented.
- ReportService was implemented.
- Bubble Sort was implemented first.
- BulkBirdGenerator was implemented.
- IHealthService and HealthService were added.

### Refactor

The code was improved while keeping the tests passing.

Examples:

- Validation logic was split into helper methods.
- Sorting was improved from Bubble Sort to MergeSort.
- Bubble Sort was kept as a comment, as required by the assignment.
- The health service was injected through an interface to support mocking.

---

## Sorting

The assignment required implementing sorting without using built-in sorting functions.

The first implementation was Bubble Sort.

After that, the active sorting algorithm was refactored to MergeSort for better performance.

Bubble Sort was kept as a comment inside `BirdSorter.cs`, as required.

The sorting function also prints the execution time in milliseconds.

---

## Mocking

The project includes an external health service simulation.

The interface is:

```csharp
public interface IHealthService
{
    bool IsBirdHealthyForSale(string ringId);
}
```

In the real application, `HealthService` returns a random true or false value.

In unit tests, Moq is used to simulate the health service.

This allows the tests to control whether the bird is approved for sale.

---

## Inventory Report

The inventory report includes:

- Total number of birds
- Average bird age
- Number of birds available for sale
- Birds sorted by hatch year in descending order

---

## Bulk Load

The system supports generating 10,000 random birds.

Each generated bird includes:

- Unique ring ID
- Bird type
- Color or mutation
- Hatch year
- Status
- Available for sale value

---

## Edge Cases

The system handles invalid input and prevents crashes.

Examples:

- Empty ring ID
- Duplicate ring ID
- Invalid color values
- Invalid hatch year
- Non-numeric hatch year input in the UI
- Empty lists
- Null lists in sorting

---

## Cross Platform

The application uses Avalonia UI, so it can run on both Mac and Windows.

---

## Important Notes

- The project is written in C# only.
- The UI is implemented using Avalonia.
- Unit tests are implemented using MSTest.
- Mocking is implemented using Moq.
- Bubble Sort was not deleted and is kept as a comment.
- MergeSort is the active sorting algorithm.