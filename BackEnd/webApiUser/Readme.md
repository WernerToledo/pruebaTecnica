# WebApiUser

API para gestionar los usuarios de ABANK. En esta API se esta manejando las operaciones CRUD, las cuales buscan administrar
de manera eficiente los datos de los usuarios.

## instalacion
Para instalar y configurar el proyecto, sigue estos pasos:

### Requisitos Previos

Asegúrate de tener los siguientes componentes instalados:

- [PostgreSQL](https://www.postgresql.org/download/)
- [VisualStudio](https://visualstudio.microsoft.com/)   

### Instalación de PostgreSQL

1. **Descargar PostgreSQL**

   Ve a la [página de descarga de PostgreSQL](https://www.postgresql.org/download/) y elige la versión adecuada para tu sistema operativo.

2. **Instalar PostgreSQL**

   Sigue las instrucciones del instalador. Durante el proceso de instalación, toma nota de los siguientes detalles:
   - **Puerto** (por defecto: `5432`)
   - **Nombre de usuario** (por defecto: `postgres`)
   - **Contraseña** (establece una contraseña segura) en mi caso fue `123`

3. **Crear la base de datos**
     
    -Abre una terminal o consola y usa el siguiente comando para crear una base de datos:

        ```bash
        psql -U postgres -c "CREATE DATABASE bdpruebatecnica;"

    - **Crear la tabla `usuarios`**:

     Conéctate a la base de datos y ejecuta el siguiente script SQL para crear la tabla `usuarios` o buscalo en la carpeta de base de datos:

     ```sql
     CREATE TABLE usuarios (
         id SERIAL PRIMARY KEY,
         nombres VARCHAR NOT NULL,
         apellidos VARCHAR NOT NULL,
         fechanacimiento DATE NOT NULL,
         direccion TEXT,
         password VARCHAR(120) NOT NULL,
         telefono VARCHAR(9) NOT NULL,
         email VARCHAR NOT NULL,
         fechacreacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
         fechamodificacion TIMESTAMP,
         CONSTRAINT telefono_valido CHECK (telefono ~ '^\d{4}-\d{4}$'),
         CONSTRAINT email_valido CHECK (
             email ~* '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
         )
     );
     ```

4. **Insertar datos en la tabla `usuarios`**:

     Ejecuta el siguiente comando SQL para insertar un registro en la tabla `usuarios`:

     ```sql
     INSERT INTO usuarios (
         nombres, 
         apellidos, 
         fechanacimiento, 
         direccion, 
         password, 
         telefono, 
         email
     ) VALUES (
         'Juan', 
         'Pérez', 
         '1990-05-15', 
         'Calle Falsa 123', 
         '123', 
         '1234-5678', 
         'juan.perez@example.com'
     );
     ```
5.


5. **Clonar el Repositorio**
   
   Clona el repositorio usando el siguiente comando:
   
   ```bash
  https://github.com/WernerToledo/pruebaTecnica.git

6. **Instala las dependencias**
    instala las dependencias usando el siguiente comando o buscalas con el la ayuda de la visual Studio en el NuGet
    -dotnet add package Dapper
    -dotnet add package Npgsql


