

## You need to work with:

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later installed
- Internet connection
- Basic knowledge of .NET CLI commands

---

## Getting Started

1. **Clone the repository:**

```bash
git clone https://github.com/Sacred6661/CrmCxode
cd CrmCxode
```
2. **Restore dependencies:**

```bash
dotnet restore
```

3. **Configure API URLs:**

Create or update the `appsettings.json` file in the project root with your API endpoints:

```json
  "MainSettings": {
    "CrmApi": "https://685dde167b57aebd2af74e12.mockapi.io/json/ticket",
    "CxoneApi": "https://d1j5ra1z1wg0000nxza0go3nhfryyyyyb.oast.pro/",
    "MaxConcurrentRequests": 2
  }
```

**Note:** You can change "CrmApi" and "CxoneApi "based on service that you will use. For now on the CrmApi is generated JSON with data and CxoneApi receive requests with data. MaxConcurrentRequests can be changed too, based on the server maximum requests.

Also you can change NLog settings in the "NLog" part of appsettings file.

4. **How to run**

```bash
dotnet run
```
This will run program, receive tickets from the "CrmApi", afer that it will send requests to the CxoneApi with mapped values.

## Contact

- For questions or feedback, contact: haker4uk@gmail.com