# TDD Notes - Bird Aviary Management

## Project Overview
This project was developed as part of a TDD assignment for managing a bird aviary.

The system supports:
- Manual bird entry
- Bulk loading of 10,000 random birds
- Inventory report generation
- Sorting birds by hatch year in descending order
- Average age calculation
- Mocking an external health service
- Basic Avalonia UI for Mac and Windows

---

## TDD Process

The project was developed using the Red-Green-Refactor cycle.

### 1. Bird Model
Red:
- Created a unit test for creating a valid Bird object.
- The test failed because Bird, BirdType and BirdStatus did not exist.

Green:
- Added Bird model.
- Added BirdType enum with at least 5 bird types.
- Added BirdStatus enum.

Refactor:
- Kept the model simple and readable.

---

### 2. BirdService
Red:
- Added tests for adding a valid bird.
- Added tests for duplicate ring IDs.
- Added tests for empty ring IDs.
- Added tests for invalid colors.
- Added tests for invalid hatch years.

Green:
- Implemented BirdService.
- Added validation logic.

Refactor:
- Split validation into private helper methods:
  - IsValidBird
  - IsDuplicateRingId
  - IsValidColorMutation
  - IsValidHatchYear

---

### 3. ReportService
Red:
- Added tests for counting birds.
- Added tests for counting birds available for sale.
- Added tests for calculating average age.

Green:
- Implemented ReportService.
- Implemented InventoryReport model.

Refactor:
- Created separate methods for each report calculation.

---

### 4. Sorting
Red:
- Added tests to verify that sorting:
  - Returns a result
  - Does not lose records
  - Sorts by hatch year descending

Green:
- Implemented Bubble Sort as the first version.

Refactor:
- Replaced the active sorting algorithm with MergeSort for better performance.
- The Bubble Sort version was kept as a comment, as required by the assignment.

---

### 5. Bulk Load
Red:
- Added tests for generating 10,000 birds.
- Added tests for unique ring IDs.
- Added tests for valid bird types, colors, statuses and hatch years.

Green:
- Implemented BulkBirdGenerator.
- Added AddBirds method to BirdService.

Refactor:
- Used predictable unique ring IDs to avoid random duplicates.

---

### 6. Mocking
Red:
- Added tests using Moq for IHealthService.
- Tested true and false responses from the mocked health service.

Green:
- Added IHealthService interface.
- Added HealthService implementation.
- Added UpdateSaleAvailability method to BirdService.

Refactor:
- Used dependency injection so BirdService can receive a mocked health service in tests.

---

## Important Assignment Requirements Covered

- C# only
- Avalonia UI
- Works on Mac and Windows
- Manual bird entry
- Unique ring ID validation
- At least 5 bird types
- Color validation with letters only
- Hatch year validation
- Bird status support
- Available for sale field
- Bulk load of 10,000 birds
- Inventory report
- Sorting by hatch year descending
- Bubble Sort kept as a comment
- Refactored sorting to MergeSort
- Sorting time printed in milliseconds
- Average age calculation with unit tests
- Mocking with IHealthService and Moq
- Edge case handling to prevent crashes