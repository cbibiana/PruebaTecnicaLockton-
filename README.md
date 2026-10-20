# Prueba Tecnica - Lockton Colombia

## 
**Bibiana Sepulveda** - Desarrolladora Full Stack .NET

## Descripcion
Solucion web funcional que permite registrar usuarios, autenticarlos mediante JWT y controlar dinamicamente los menus disponibles para cada usuario.

## Tecnologias Utilizadas
- .NET 10.0 (Framework backend)
- ASP.NET Core Web API (API REST)
- Entity Framework Core 10.0 (ORM)
- SQL Server (Base de datos relacional)
- BCrypt.Net (Hash de contrasenas)
- JWT Bearer (Autenticacion por tokens)
- Scalar (Documentacion de API)
- HTML/CSS/JavaScript (Frontend)

## Arquitectura
El proyecto sigue el patron Clean Architecture con 3 capas:
- PruebaTecnica.API: Capa de presentacion (Controllers, Frontend)
- PruebaTecnica.Core: Capa de dominio (Entidades, Interfaces, DTOs)
- PruebaTecnica.Infrastructure: Capa de infraestructura (Data, Repositorios)

## Modelo de Base de Datos
- Usuarios: Id, Nombre, Correo (unico), PasswordHash, Activo, FechaCreacion
- Menus: Id, Nombre, Descripcion, Ruta, Activo
- UsuarioMenus: UsuarioId, MenuId (relacion muchos a muchos)

## Endpoints de la API
- POST /api/auth/login - Autenticar usuario (Sin autenticacion)
- POST /api/users - Crear usuario (Sin autenticacion)
- GET /api/users - Listar usuarios (Requiere JWT)
- GET /api/users/{id} - Obtener usuario por ID (Requiere JWT)
- GET /api/users/{id}/menus - Consultar menus de un usuario (Requiere JWT)
- PUT /api/users/{id}/menus - Asignar menus a un usuario (Requiere JWT)
- GET /api/menus - Consultar todos los menus (Requiere JWT)
- GET /api/menus/mis-menus - Consultar menus del usuario autenticado (Requiere JWT)

## Requisitos para Ejecutar
1. .NET SDK 10.0 o superior
2. SQL Server (LocalDB, Express o Developer)
3. Visual Studio 2026 o Visual Studio Code

## Instalacion y Ejecucion
1. Clonar el repositorio
2. Editar PruebaTecnica.API/appsettings.json y cambiar el nombre del servidor SQL
3. Crear la base de datos: ejecutar ScriptBD.sql en SQL Server Management Studio o usar Entity Framework (dotnet ef database update)
4. Ejecutar: cd PruebaTecnica.API y luego dotnet run
5. Abrir en el navegador: https://localhost:7274 (Frontend) o https://localhost:7274/scalar/v1 (Documentacion API)

## Credenciales de Prueba
- Admin / admin@gmail.com / 123456 / Menus: Dashboard, Usuarios, Reportes, Configuracion
- Bibiana / bibiana@gmail.com / 123 / Menus: Usuarios
- Pedro / pedro@gmail.com / 123456 / Menus: Dashboard, Reportes

## Seguridad
- Contrasenas almacenadas con BCrypt (hash + salt)
- Autenticacion mediante JWT Bearer Token
- Endpoints protegidos con atributo [Authorize]
- Control de acceso en backend (no solo en frontend)
- Validacion de datos de entrada

## Herramientas de IA Utilizadas
- Amazon Q: Utilizada como asistente para consultas sobre configuracion de .NET 10, resolucion de errores y estructura del proyecto. Todo el codigo fue comprendido, revisado y adaptado por la candidata.

## Escenario de Validacion
1. Iniciar sesion con admin@gmail.com (ve 4 menus)
2. Iniciar sesion con pedro@gmail.com (ve 2 menus: Dashboard, Reportes)
3. Iniciar sesion con bibiana@gmail.com (ve 1 menu: Usuarios)
4. Intentar acceder a un endpoint protegido sin token (devuelve 401 Unauthorized)
