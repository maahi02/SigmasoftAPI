# Project Sigmasoft

## Overview
This project is built using **Clean Architecture** principles, ensuring separation of concerns and promoting scalability and testability. The solution comprises distinct layers for Application, Domain, Infrastructure, and Web API.

## Technologies Used
- **.NET 7** (Web API)
- **Entity Framework Core** (for data access)
- **Microsoft SQL Server** (database)
- **Swagger/OpenAPI** (API documentation)
- **Dependency Injection** (for service and repository management)
- **Memory Cache** (for caching purposes)

## Design Patterns
The project employs the following design patterns:

1. **Repository Pattern**
   - Encapsulates data access logic.
   - Repository Interface: `IRepository<T>`
   - Implementation: `CandidateRepository`

2. **Service Layer Pattern**
   - Encapsulates business logic.
   - Interface: `ICandidateService`
   - Implementation: `CandidateService`

3. **Middleware Pattern**
   - Custom global exception handling via `GlobalExceptionMiddleware`.

## Features
- **Entity Framework Core Integration:**
  - Configured with SQL Server using a connection string from `appsettings.json`.
  - Automatic migration application during application startup.

- **Dependency Injection:**
  - Services and repositories are registered in `Program.cs` to promote decoupling and testability.

- **Swagger/OpenAPI Documentation:**
  - Enabled for seamless API testing and exploration during development.

- **CORS Support:**
  - Configured to allow cross-origin requests with `app.UseCors("*")`.

- **Global Exception Handling:**
  - Middleware implementation to handle exceptions and provide consistent error responses.

## Folder Structure
```
- Application
  - Interfaces
    - Content
- Domain
  - Models
- Infrastructure
  - Content
    - Data
    - Repository
    - Services
  - Middleware
- WebAPI (SigmaSoftAPI)
```

## Program Initialization (`Program.cs`)
### Key Configurations
1. **Database Context Registration:**
   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(connectionString));
   ```

2. **Repository and Service Registration:**
   ```csharp
   builder.Services.AddScoped<IRepository<Candidate>, CandidateRepository>();
   builder.Services.AddScoped<ICandidateService, CandidateService>();
   ```

3. **Swagger Configuration:**
   ```csharp
   builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
   });
   ```

4. **Middleware Usage:**
   ```csharp
   app.UseMiddleware<GlobalExceptionMiddleware>();
   ```

5. **Migrations:**
   Automatic migration application during startup:
   ```csharp
   using (IServiceScope? scope = app.Services.CreateScope())
   {
       var service = scope.ServiceProvider;
       var appDbContext = service.GetRequiredService<AppDbContext>();
       await appDbContext.Database.MigrateAsync();
   }
   ```
   # It create a database automatic.

## How to Run
1. Clone the repository.
2. Set up the database connection string in `appsettings.json` under `ConnectionStrings:DefaultConnection`.
3. Run the application.
4. Access Swagger UI at `https://localhost:<port>/swagger` for API testing.

## Future Improvements
- Add unit tests for services and controllers.
- Implement proper CORS policy for production.
- Enhance exception handling for more detailed error responses.
- Introduce logging to a centralized system like Serilog or Application Insights.
- Refactor for better compliance with SOLID principles where applicable.
- Optimize performance of database queries and introduce caching for frequently accessed data.
- Can use dapper and StoreProcedure for queries or database operation.


## Contact
For any questions or issues, please contact at mahesh3thakur@gmail.com

