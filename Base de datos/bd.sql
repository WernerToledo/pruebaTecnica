create database bdpruebatecnica

creacion de la tabla usuarios

CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    nombres VARCHAR NOT NULL,
    apellidos VARCHAR NOT NULL,
    fechanacimiento DATE NOT NULL, 
    direccion text,
    password VARCHAR(120) NOT NULL,
    telefono VARCHAR(9) NOT NULL UNIQUE,
    email VARCHAR NOT NULL,
    fechacreacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fechamodificacion TIMESTAMP
	CONSTRAINT telefono_valido CHECK (telefono ~ '^\d{4}-\d{4}$')
 	CONSTRAINT email_valido CHECK (
        email ~* '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
    )
);



