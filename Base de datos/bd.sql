--CREATE DATABASE bdpruebatecnica;

\c bdpruebatecnica

CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    nombres VARCHAR NOT NULL,
    apellidos VARCHAR NOT NULL,
    fechanacimiento DATE NOT NULL, 
    direccion TEXT,
    password VARCHAR(120) NOT NULL,
    telefono VARCHAR(9) NOT NULL UNIQUE,
    email VARCHAR NOT NULL,
    fechacreacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fechamodificacion TIMESTAMP,
    CONSTRAINT telefono_valido CHECK (telefono ~ '^\d{4}-\d{4}$'),
    CONSTRAINT email_valido CHECK (email ~* '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$')
);

-- Inserta un registro de prueba
INSERT INTO usuarios (nombres, apellidos, fechanacimiento, direccion, password, telefono, email)
VALUES ('nuevocop22i', 'nuevcop22i', '2024-09-14', 'nuevo nuevo', '$2a$11$qaCucIxdUfKDcEZ6F1FkbeZL33yFPAMMtHvihnDk1sRWszsoFv/Di', '3329-2352', 'user@example.com');
