
# iCloud.Dav

## Description
**iCloud.Dav** is a .NET library that allows developers to interact with various iCloud services, including **Contacts**, **Calendars**, **Events**, **Reminder Lists**, and **Reminders**. The library integrates seamlessly with .NET projects and supports easy authentication using **iCloud.Dav.Auth**.

## Installation

You can install **iCloud.Dav** packages using the **NuGet** package manager, or by downloading and building the source code manually.

### NuGet Installation

To install the iCloud.Dav libraries via NuGet, use the following commands:

- To install **iCloud.Dav.Core**, run:
  ```bash
  Install-Package iCloud.Dav.Core
  ```
- To install **iCloud.Dav.Auth**, run:
  ```bash
  Install-Package iCloud.Dav.Auth
  ```
- To install **iCloud.Dav.Calendar**, run:
  ```bash
  Install-Package iCloud.Dav.Calendar
  ```
- To install **iCloud.Dav.People**, run:
  ```bash
  Install-Package iCloud.Dav.People
  ```

### Manual Installation

1. Download the source code from this repository.
2. Open the solution file (`.sln`) in Visual Studio.
3. Build the solution.

## Usage

To use the **iCloud.Dav** libraries in your .NET project, add a reference to the respective assembly (either via **NuGet** or by referencing the built assembly directly).

## Examples

You can find detailed examples for all major operations in the [`examples/`](https://github.com/gachris/iCloud.Dav/tree/master/examples) directory of this repository.

## Contributing

Contributions to **iCloud.Dav** are welcome! If you want to contribute to the project, please fork the repository, create a new branch, and open a pull request with your changes.

## License

**iCloud.Dav** is licensed under the MIT license. See the [License](https://github.com/gachris/iCloud.Dav/tree/master/LICENSE.txt) file for more information.