# Contact Book - Disjoint Set Union (DSU) Implementation

This project is a C# Console Application developed on **macOS** using **.NET 8.0**. It manages a contact list and utilizes Graph Theory through the Disjoint Set Union (DSU) algorithm to detect and group duplicate contacts based on shared phone numbers or email addresses.

## Features
*   **Graph-Based Deduplication**: Implements DSU with Path Compression to identify clusters of related contacts.
*   **Modern C# Syntax**: Uses Nullable Reference Types (`string?`) and Null-coalescing operators (`??`) to ensure memory safety and prevent null-reference exceptions.
*   **Data Management**: Supports real-time searching, filtering, and sorting by last name.

## Technical Details
*   **Algorithm**: Disjoint Set Union (DSU) / Union-Find.
*   **Path Compression**: Optimized `FindRoot` method to improve performance during cluster lookups.
*   **Environment**: Developed and tested on macOS.

## How to Run
1. Ensure you have the .NET SDK installed.
2. Open your terminal in the project directory.
3. Run the following command: ## dotnet run
