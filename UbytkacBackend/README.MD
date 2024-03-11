
Full Database model to project model
	Scaffold-DbContext "Server=95.183.52.33;Database=hotels;User ID=sa;Password=Hotel2023+;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DBModel

Initite Migrations files to DOTNET migration management for Upload/Manage Database Migrations
Add-Migration Initial -Context Company.WebApplication1.Data.ApplicationDbContext

Upload migrations to DATABASE
	dotnet ef database update

Export Database model to SQL script
dotnet ef dbcontext script --output Migrations/BasicDBModel.sql


Change Password to encrypt
isValid = BCrypt.Net.BCrypt.Verify(word, hashedWordFromDB);

Translations
TranslateService.DBTranslate("IncorrectEmailOrPassword", language.Language)

DataModel context with include another table need in serialize:
, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true }


web Pages used
DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
example:
       return JsonSerializer.Serialize(rootData, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true, 
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

To generate a developer certificate run 'dotnet dev-certs https'. 
To trust the certificate (Windows and macOS only) run 'dotnet dev-certs https --trust'.

---

Windows Installation Steps

Run Installation Exe file
Check "config.txt" Project file for correct connection string with allowed Create Database
Copy the License 'license.lic' file to "Data" folder
Is was installed as Windows Service - you can enable/disable over services
Run the server and Done

cs

---

Linux Installation Steps

Install ASPNETCORE6 Runtime support on your Linux Server
Copy unpacked Project to some folder on Linux server
Configure and enable new linux service dotnet-ProjectManagement.service
Check "config.txt" Project file in "Data" folder for correct connection string with allowed Create Database
Copy the License 'license.lic' file to "Data" folder
Run the server and Done

```cs

Copy new service file dotnet-ProjectManagement.service to '/lib/systemd/system' Linux server folder

enable service by command: systemctl enable dotnet-ProjectManager.service run service by command: systemctl start dotnet-ProjectManager.service

stop service by command: systemctl stop dotnet-ProjectManager.service status service by command: systemctl status dotnet-ProjectManager.service ```

