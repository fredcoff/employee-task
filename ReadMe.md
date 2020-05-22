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
  "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EmployeeTaskTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
  },
```

```json
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=192.168.101.200;Persist Security Info=True;User ID=sa;Password=aaaa;Database=EmployeeTaskTestDb;MultipleActiveResultSets=true;"
  },
```

[Comments are not allowed in JSON.](https://stackoverflow.com/questions/244777/can-comments-be-used-in-json)  
But it appears that we can do so in C#. 🙃




