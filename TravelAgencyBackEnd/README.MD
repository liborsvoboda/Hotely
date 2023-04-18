
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