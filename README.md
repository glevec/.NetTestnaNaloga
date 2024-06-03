## .NET Testna Naloga
### Installation

1. **Clone the repository**
```bash
   git clone  https://github.com/glevec/.NetTestnaNaloga.git
```

2. **Navigate to the project folder**
```bash
   cd TestnaNaloga
```

3. **Install neccessary packages**
```bash
   dotnet restore
```

4. **Setup database**
Change the DefaultConnection in appsettings.json to point to your local sqldb

5. **Run the following commands**
```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   dotnet build
   dotnet run
```

### Swagger documentation and Postman
- The swagger documentation should be available at the following url: http://localhost:5147/swagger/index.html
- Postman collection is located in the root of the project
