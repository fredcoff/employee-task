# EmployeeTask

## Database Migrations

Add a migration

`dotnet ef migrations add InitialCreate -o Persistence/Migrations --project src/Infrastructure --startup-project src/WebUI`

Remove the last migration

`dotnet ef migrations remove --project src/Infrastructure --startup-project src/WebUI`


*Database settings*

`appsettings.json`

```json
  // "UseInMemoryDatabase": true,
  // "ConnectionStrings": {
  //     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EmployeeTaskTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
  // },
```

```json
<add connectionString="Data Source=192.168.101.200;Initial Catalog=DiocLC_GivingTrendDB;Persist Security Info=True;User ID=sa;Password=aaaa" name="dashboardConnectionString" providerName="System.Data.SqlClient"/>
```


