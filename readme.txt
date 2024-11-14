+--------------------------+
|      Presentation        |
|  (src/Web)               |
|                          |
| - Controllers            |
| - Views                  |
| - Models                 |
| - wwwroot                |
| - Startup.cs             |
+------------+-------------+
             |
             v
+------------+-------------+
|       Application        |
|  (src/Application)       |
|                          |
| - Commands               |
| - Queries                |
| - Handlers               |
+------------+-------------+
             |
             v
+------------+-------------+
|        Domain            |
|  (src/Domain)            |
|                          |
| - Entities               |
| - Interfaces             |
| - ValueObjects           |
+------------+-------------+
             |
             v
+------------+-------------+
|     Infrastructure       |
|  (src/Infrastructure)    |
|                          |
| - Data                   |
|   - ApplicationDbContext |
| - Repositories           |
| - PaymentGateways        |
| - Services               |
+------------+-------------+
             |
             v
+------------+-------------+
|         Shared           |
|    (src/Shared)          |
|                          |
| - DTOs                   |
| - Helpers                |
| - Extensions             |
+--------------------------+
             |
             v
+------------+-------------+
|          Tests           |
|     (src/Tests)          |
|                          |
| - UnitTests              |
| - IntegrationTests       |
+--------------------------+
             |
             v
+------------+-------------+
|           build          |
|       (build)            |
|                          |
| - scripts                |
+--------------------------+


Presentation Layer (src/Web): 
This is the user interface layer where your controllers, views, and models reside. It's responsible for handling HTTP requests and responses.

Application Layer (src/Application): 
This layer handles the application logic and coordinates activities between the domain and infrastructure layers. It includes commands, queries, and handlers.

Domain Layer (src/Domain): 
The core of the application containing business logic. It includes entities, value objects, and interfaces. This layer is independent and should not depend on other layers.

Infrastructure Layer (src/Infrastructure): 
This layer contains classes for data access, external services, and other infrastructure concerns. It implements the interfaces defined in the domain layer.

Shared (src/Shared): 
This layer includes shared resources such as DTOs, helpers, and extensions that can be used across different layers.

Tests (src/Tests): 
This layer includes unit and integration tests to ensure your application works as expected.

Build (build): 
This folder includes build scripts and configurations.
