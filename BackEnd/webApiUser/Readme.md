# Documentación del Proyecto API de Gestión de Usuarios

## Descripción del Proyecto

Este proyecto es una API REST construida con .NET 8, diseñada para la gestión de usuarios. La API permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre una tabla de usuarios. En algunos casos la las rutas tienes seguridad. La base de datos utilizada es PostgreSQL, que se maneja mediante contenedores Docker.

## Tecnologías y Herramientas Utilizadas

### .NET 8

.NET 8 es el framework principal utilizado para desarrollar la API. Para instalar .NET 8, se ha utilizado Visual Studio.

**Instalación en Visual Studio:**
1. **Descargar Visual Studio:**
   - Visita [la página oficial de Visual Studio](https://visualstudio.microsoft.com/) y descarga el instalador.
2. **Instalar:**
   - Ejecuta el instalador y selecciona el componente **.NET desktop development** y **ASP.NET and web development**.
   - Completa la instalación siguiendo las instrucciones en pantalla.
3. **Verificar Instalación:**
   - Abre Visual Studio y verifica que se pueda crear un proyecto de .NET 8.

### Docker

Docker se utiliza para contenerizar PostgreSQL y facilitar el despliegue de la base de datos.

**Instalación:**
1. **Descargar Docker Desktop:**
   - Visita [la página oficial de Docker](https://www.docker.com/products/docker-desktop) y descarga Docker Desktop.
2. **Instalar:**
   - Ejecuta el instalador descargado y sigue las instrucciones en pantalla.
3. **Verificar Instalación:**
   - Abre una terminal y ejecuta `docker --version` para verificar que Docker está correctamente instalado.

4. **Extensiones de .NET Core Utilizadas:**
   - **BCrypt.Net-Next**: Para la gestión segura de contraseñas mediante hashing.
   - **Microsoft.AspNetCore.Authentication.JwtBearer**: Para la autenticación basada en JSON Web Tokens (JWT).
   - **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore**: Para el diagnóstico y manejo de errores de Entity Framework Core.
   - **Microsoft.EntityFrameworkCore**: ORM utilizado para interactuar con la base de datos PostgreSQL.
   - **Microsoft.VisualStudio.Web.CodeGeneration.Design**: Herramienta para la generación de código en proyectos ASP.NET Core.
   - **Npgsql**: Proveedor de datos para PostgreSQL en .NET.
   - **Swashbuckle.AspNetCore**: Para generar documentación Swagger de la API.
   - **System.IdentityModel.Tokens.Jwt**: Para el manejo y validación de tokens JWT.

### Endpoints:
**POST /api/v1/auth/login/login**

- **Descripción:** Maneja la solicitud de login para autenticar un usuario y generar un token.
- **Autenticación Requerida:** No (este endpoint es utilizado para autenticar y obtener un token).
- **Cuerpo de la Solicitud:**
  ```json
  {
    "telefono": "####-####",
    "password": "contraseña_hash"
  }


- **POST /api/v1/usuarios**
  - **Descripción:** Crea un nuevo usuario en la base de datos.
  - **Autenticación Requerida:** Sí (se requiere autenticación para crear un usuario)
  - **Cuerpo de la Solicitud:**
    ```json
    {
        "nombres": "string",
        "apellidos": "string",
        "fechanacimiento": "2024-09-14T20:00:12.318Z",
        "direccion": "string",
        "password": "string",
        "telefono": "0170-1892",
        "email": "user@example.com",
    }
    ```
  - **Respuesta Exitosa:**
    - **Código de Estado:** 201 Created
    - **Ubicación del Nuevo Recurso:** `Location: /api/v1/usuarios/{id}`

- **GET /api/v1/usuarios**
  - **Descripción:** Recupera una lista de todos los usuarios.
  - **Autenticación Requerida:** No
  - **Respuesta Exitosa:**
    - **Código de Estado:** 200 OK
    - **Cuerpo:** Lista de objetos `usuario` en formato JSON.

- **GET /api/v1/usuarios/{id}**
  - **Descripción:** Recupera un usuario específico por su ID.
  - **Autenticación Requerida:** Sí (se requiere autenticación para acceder a un usuario específico)
  - **Parámetros de Ruta:**
    - `id` (int): ID del usuario a recuperar.
  - **Respuesta Exitosa:**
    - **Código de Estado:** 200 OK
    - **Cuerpo:** Objeto `usuario` en formato JSON.
  - **Respuesta si no se encuentra el Usuario:**
    - **Código de Estado:** 404 Not Found

- **PUT /api/v1/usuarios/{id}**
  - **Descripción:** Actualiza la información de un usuario existente por su ID.
  - **Autenticación Requerida:** Sí (se requiere autenticación para actualizar un usuario)
  - **Parámetros de Ruta:**
    - `id` (int): ID del usuario a actualizar.
  - **Cuerpo de la Solicitud:**
    ```json
    {
        "nombres": "string",
        "apellidos": "string",
        "fechanacimiento": "2024-09-14T20:00:12.318Z",
        "direccion": "string",
        "password": "string",
        "telefono": "0170-1892",
        "email": "user@example.com"
    }
    ```
  - **Respuesta Exitosa:**
    - **Código de Estado:** 204 No Content

- **DELETE /api/v1/usuarios/{id}**
  - **Descripción:** Elimina un usuario específico por su ID.
  - **Autenticación Requerida:** Sí (se requiere autenticación para eliminar un usuario)
  - **Parámetros de Ruta:**
    - `id` (int): ID del usuario a eliminar.
  - **Respuesta Exitosa:**
    - **Código de Estado:** 204 No Content

### Explicación de docker-compose.yml
  -docker-compose.yml es un archivo que sirve para tener el entorno de la base de datos en postgres este se encargara de instalar una imagen de la base de datos para tener el servicio.
  
  **Usar el servicio**
  1.**Abre la terminar de windows y corre el siguiente comando**
      **docker-compose up**