# NameSorter Solution

## Overview
The NameSorter solution is designed to sort a list of names first by last name, then by any given names a person may have. It demonstrates the use of .NET Core, dependency injection, interfaces, and LINQ for sorting and file operations, adhering to SOLID principles for scalable and maintainable code.

## Solution Structure
The solution is divided into several projects, each serving a specific role:

- **NameSorterCore**: Contains the core logic for name validation, sorting strategy, and file system operations.
- **NameSorterApp**: A console application that serves as the entry point, orchestrating the flow from taking input, processing names, and outputting the sorted list.
- **NameSorterCore.Tests**: Unit tests covering the core functionalities of the solution.

## Key Components

### Models
- **PersonName**: Represents a person's name, including last name and given names.
- **AppSettings**: Holds configuration settings, such as the output file path.

### Interfaces
- **IFileProcessor**: Defines file processing operations.
- **IFileSystem**: Abstracts file system operations, allowing for reading and writing files.
- **INameSorter**: Defines the contract for sorting names.
- **INameValidator**: Provides validation for name formats.
- **ISortStrategy**: Specifies the sorting strategy interface.
- **INameParser**: Responsible for parsing individual name strings into `PersonName` objects, ensuring they adhere to the expected format.
- **INameSorterService**: An essential service interface for orchestrating the name sorting process.

### Implementations
- **TextProcessor**: Implements `IFileProcessor`, orchestrating the file processing workflow.
- **FileSystem**: Implements `IFileSystem`, providing methods to interact with the file system.
- **NameValidator**: Implements `INameValidator`, validating the format of names.
- **NameSorter**: Implements `INameSorter`, parsing and sorting names using a specified sorting strategy.
- **LinqSortStrategy**: Implements `ISortStrategy`, providing a LINQ-based sorting mechanism.
- **NameParser**: Implements `INameParser`, parsing raw name strings into structured `PersonName` instances.
- **NameSorterService**: Implements name sorting, including validation and applying sorting strategies.
### Program Entry
- **Program.cs**: The main entry point of the application, which sets up dependency injection and processes the input file.

## Setup and Configuration
The application uses .NET Core's built-in dependency injection and configuration systems. Settings like the output file path are configured in `appsettings.json`.

### Dependencies
- .NET Core
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Configuration

### Running the Application
1. Ensure you have .NET Core 8 installed.
2. Navigate to the `NameSorterApp` directory.
3. Run `dotnet run -- <path to unsorted names file>` to execute the application.

## Testing
Unit tests are provided within the `NameSorterCore.Tests` project. To run these tests:
1. Navigate to the `NameSorterCore.Tests` directory.
2. Run `dotnet test` to execute the tests.

## Future Improvements

While the current implementation of the NameSorter solution addresses the primary requirements efficiently, there are several areas where future improvements can significantly enhance its functionality, performance, and user experience. Here are some of the key areas targeted for future development:

### Enhanced Validation Logic
- **Complex Name Handling**: Enhancements to the `NameValidator` to support a wider variety of name formats, including titles & suffixes.
- **Customizable Validation Rules**: Implementation of a system allowing users to define custom validation rules to accommodate specific naming conventions or requirements.

### Sorting Algorithm Enhancements
- **Performance Optimization**: Investigation and adoption of more efficient sorting algorithms for handling large datasets, along with benchmarking to identify optimal solutions under various scenarios.
- **Stable Sorting**: Ensuring that the sorting algorithm preserves the original order of names with identical sort keys, maintaining the integrity of the input data.

### User Experience and Usability
- **Interactive CLI Mode**: Development of an interactive mode that guides users through the sorting process with prompts and helpful feedback.
- **Improved Error Handling**: More descriptive and actionable error messages to help users quickly identify and resolve issues.
- **Logging Integration**: Integration of a logging framework for detailed operational logging, aiding in troubleshooting and monitoring.

### Configurability and Flexibility
- **Dynamic Sorting Criteria**: Allowing users to configure the sorting criteria dynamically, such as prioritizing different name parts.
- **Output Formatting Options**: Options for customizing the output format, adapting the sorted list to various presentation requirements.

### Scalability and Performance
- **Parallel Processing**: Leveraging parallel processing to improve performance on large datasets, making efficient use of multi-core processors.
- **Memory Usage Optimization**: Techniques for reducing memory footprint, crucial for processing large files.

### Testing and Quality Assurance
- **Expanded Test Coverage**: Broadening the test suite to cover more edge cases, ensuring high reliability and stability of the solution.
- **Continuous Integration (CI) Pipeline**: Implementation of a CI pipeline to automate testing and ensure consistent quality across releases.

### Documentation and Community
- **Comprehensive User Documentation**: Development of detailed user guides, examples, and FAQs to enhance user support.

These future improvements aim to make the NameSorter solution not just a tool for sorting names but a comprehensive system adaptable to a wide range of use cases, user preferences, and environments, contributing to its longevity and usefulness.

## Contributions
Contributions are welcome. Please fork the repository, make your changes, and submit a pull request.

## Author

❤️ Hussam ❤️