
3. Build & run
- From command line:
  dotnet build
  dotnet run --project TaskManagementSystem

- From Visual Studio 2022:
  - Open solution, then __Build > Build Solution__
  - To run: __Debug > Start Debugging__ or press F5

API & Endpoints (high level)
- Authentication
  - `POST /api/auth/register` — register a new employee (if implemented)
  - `POST /api/auth/login` — returns JWT token

- Employees
  - `GET /api/employee` — list employees
  - `GET /api/employee/{id}` — employee details
  - `POST /api/employee` — create employee
  - `PUT /api/employee/{id}` — update employee
  - `DELETE /api/employee/{id}` — delete employee

- Projects
  - `GET /api/project` — list projects
  - `GET /api/project/{id}` — project details and tasks
  - `POST /api/project` — create project
  - `PUT /api/project/{id}` — update project

- Tasks
  - `GET /api/task` — list tasks
  - `GET /api/task/{id}` — task details with sub-tasks
  - `POST /api/task` — create task (accepts Task DTO)
  - `PUT /api/task/{id}` — update task
  - Subtask endpoints are available under `SubTask` controller

Swagger UI
- When running locally, Swagger UI is typically available at:
  `https://localhost:{port}/swagger` — use it to inspect and exercise endpoints.

Database migrations
- If using EF Core migrations:
  - Add migration:
    dotnet ef migrations add InitialCreate --project TaskManagement.infrastructure --startup-project TaskManagementSystem
  - Update database:
    dotnet ef database update --project TaskManagement.infrastructure --startup-project TaskManagementSystem

AutoMapper
- DTOs live under `TaskManagementSystem/DtoModels` and AutoMapper profiles configured in `AutoMapperProfile.cs`.

Caching
- Redis integration and caching extensions are available (see `RedishExtention` and `CacheService`). Ensure Redis connection string is set in config to enable.

Configuration and environment
- Keep secrets (JWT key, DB passwords) out of source control. Use environment variables or a secrets store in production.
- App configuration is read in `Program.cs`. Look for configuration keys:
  - `ConnectionStrings:DefaultConnection`
  - `Jwt:*`
  - `Redis:Connection`

Testing
- If tests are present, run them with:
  dotnet test

Development notes & architecture
- Layered architecture:
  - Controllers (API surface)
  - Domain services (business logic)
  - Infrastructure repositories (data access)
  - DTOs and AutoMapper for mapping between domain models and API models
- Central exception handling is implemented via middleware (`MiddelWare/ExceptionMiddleware.cs`) to return consistent error responses.
- Response models: `DataSuccessResponseModel` and `DataErrorResponseModel` used to standardize API responses.

Contribution
- Fork the repository
- Create a feature branch
- Add tests for new behavior
- Open a Pull Request describing changes

Recommended VS settings
- Set __Tools > Options > Text Editor > C# > Code Style__ to match project preferences
- Use __Analyze > Run Code Analysis on Solution__ before creating PRs

Common troubleshooting
- If migrations fail, verify the connection string and the target database.
- If JWT authentication fails, ensure `Jwt:Key` matches token generation and validation configuration.
- If Redis caching not working, verify `Redis:Connection` and that Redis server is reachable.

License
- Include the license file in the repo root (e.g., MIT). Update this section with the chosen license.

Contact
- For issues, open GitHub Issues on the repository.

---

If you want, I can:
- Produce a complete `README.md` tailored to this repository with exact examples (connection string samples, full endpoint list extracted from controllers), or
- Generate a skeleton `docker-compose` with SQL Server and Redis for local development.
