# OrderManagementAPI

API REST profesional para un sistema de gestión de órdenes con clientes, productos y detalles de orden.

## Requisitos Previos

Para ejecutar este proyecto necesitas:

* .NET 8 SDK
* SQL Server (Azure SQL, AWS RDS o instancia local)
* Visual Studio 2022 o VS Code

## Configuración y Ejecución

1. Clonar el repositorio.
2. Configurar la cadena de conexión en `appsettings.json`. (OPCIONAL : La Api cuenta con una bdd en la nube ) 
   * El proyecto soporta tanto bases de datos locales como en la nube .
   * Asegúrese de que la cadena de conexión apunte a su instancia (Cloud o Local).
3. Ejecutar el script de base de datos (`script.sql`) en su instancia de SQL Server para crear las tablas y procedimientos almacenados.
4. Ejecutar la aplicación:

   ```bash
   dotnet run
   ```

5. Acceder a Swagger para probar los endpoints: `http://localhost:5015/swagger`

## Stack Tecnológico

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core (Database First / SQL Raw)
* SQL Server

## Decisiones Técnicas

### Arquitectura en Capas
Se implementó una arquitectura limpia separada en capas para garantizar mantenibilidad y escalabilidad:

* **Controllers**: Manejan las peticiones HTTP y respuestas.
* **Services**: Contienen la lógica de negocio y validaciones complejas.
* **Repositories**: Abstraen el acceso a datos.
* **DTOs**: Objetos de transferencia de datos para desacoplar modelos de base de datos de la API pública.
* **Models**: Entidades de EF Core que mapean a la base de datos.

### Stored Procedures
Se utilizaron Stored Procedures para todas las operaciones críticas (CRUD y Transacciones) para maximizar el rendimiento y la seguridad.
* Clientes: `sp_InsertarCliente`, `sp_ActualizarCliente`, etc.
* Productos: `sp_ObtenerProductos`, etc.
* Órdenes: `sp_CreateOrden` (Maneja transacción compleja con XML para detalles).

### FluentValidation
Se utilizó FluentValidation para separar las reglas de validación de los modelos, permitiendo reglas más complejas y mantenibles.
* Validación de unicidad y formatos (identidad).
* Validación de negocio (precios positivos, stock no negativo).

### AutoMapper
Se utilizó AutoMapper para simplificar la transformación entre Entidades y DTOs, reduciendo el código repetitivo en los repositorios.

## Endpoints Principales

### Clientes
* `GET /api/clientes`: Listar todos.
* `GET /api/clientes/{id}`: Obtener por ID.
* `POST /api/clientes`: Crear cliente.
* `PUT /api/clientes/{id}`: Actualizar cliente.

### Productos
* `GET /api/productos`: Listar todos.
* `GET /api/productos/{id}`: Obtener por ID.
* `POST /api/productos`: Crear producto.
* `PUT /api/productos/{id}`: Actualizar producto.

### Órdenes
* `POST /api/ordenes`: Crear orden completa.
    * Maneja transacciones ACID.
    * Valida existencias (devuelve error detallado con stock disponible vs solicitado).
    * Cálculo automático de impuestos y totales.
