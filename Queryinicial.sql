------------------------------------------------------------
-- CERRAR CONEXIONES Y BORRAR BD SI EXISTE
------------------------------------------------------------
IF DB_ID('SistemaBusesDB') IS NOT NULL
BEGIN
    ALTER DATABASE SistemaBusesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SistemaBusesDB;
END
GO

------------------------------------------------------------
-- CREAR BASE DE DATOS
------------------------------------------------------------
CREATE DATABASE SistemaBusesDB;
GO

USE SistemaBusesDB;
GO

------------------------------------------------------------
-- TABLAS
------------------------------------------------------------

-- EMPRESA
CREATE TABLE Empresa (
    EmpresaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Pais VARCHAR(50),
    Telefono VARCHAR(20)
);

-- BUS
CREATE TABLE Bus (
    BusId INT IDENTITY(1,1) PRIMARY KEY,
    EmpresaId INT NOT NULL,
    Placa VARCHAR(20),
    Modelo VARCHAR(50),
    Capacidad INT,
    FOREIGN KEY (EmpresaId) REFERENCES Empresa(EmpresaId)
);

-- CONDUCTOR
CREATE TABLE Conductor (
    ConductorId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(80),
    Licencia VARCHAR(30),
    Telefono VARCHAR(20)
);
CREATE TABLE Ventas (
    VentaId INT IDENTITY(1,1) PRIMARY KEY,
    AsientoId INT NOT NULL,
    RutaId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE()
    -- Opcional: UsuarioId INT, Precio DECIMAL(18,2), etc.
);


-- RUTA
CREATE TABLE Ruta (
    RutaId INT IDENTITY(1,1) PRIMARY KEY,
    Origen VARCHAR(80) NOT NULL,
    Destino VARCHAR(80) NOT NULL,
    DuracionMin INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    ImagenUrl VARCHAR(255) NULL,       -- URL de la imagen de la ruta
    OrigenLat DECIMAL(9,6) NULL,       -- Latitud del origen
    OrigenLng DECIMAL(9,6) NULL,       -- Longitud del origen
    DestinoLat DECIMAL(9,6) NULL,      -- Latitud del destino
    DestinoLng DECIMAL(9,6) NULL       -- Longitud del destino
);


-- ITINERARIO
CREATE TABLE Itinerario (
    ItinerarioId INT IDENTITY(1,1) PRIMARY KEY,
    RutaId INT,
    BusId INT,
    ConductorId INT,
    Fecha DATE,
    HoraSalida TIME,
    HoraLlegada TIME,
    FOREIGN KEY (RutaId) REFERENCES Ruta(RutaId),
    FOREIGN KEY (BusId) REFERENCES Bus(BusId),
    FOREIGN KEY (ConductorId) REFERENCES Conductor(ConductorId)
);

CREATE TABLE Usuario (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(80) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Telefono VARCHAR(20),
    Password VARCHAR(255) NOT NULL
);



-- BOLETO
CREATE TABLE Boleto (
    BoletoId INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT,
    ItinerarioId INT,
    FechaCompra DATE,
    Asiento VARCHAR(10),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (ItinerarioId) REFERENCES Itinerario(ItinerarioId)
);

-- OFERTA
CREATE TABLE Oferta (
    OfertaId INT IDENTITY(1,1) PRIMARY KEY,
    EmpresaId INT,
    Titulo VARCHAR(100),
    Descripcion VARCHAR(255),
    Descuento INT,
    Vigente BIT DEFAULT 1,
    FOREIGN KEY (EmpresaId) REFERENCES Empresa(EmpresaId)
);

-- RESENA (antes Reseña)
CREATE TABLE Resena (
    ResenaId INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT,
    RutaId INT,
    Calificacion INT,
    Comentario VARCHAR(255),
    Fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (RutaId) REFERENCES Ruta(RutaId)
);

-- REPORTE
CREATE TABLE Reporte (
    ReporteId INT IDENTITY(1,1) PRIMARY KEY,
    BusId INT,
    ConductorId INT,
    Descripcion VARCHAR(255),
    Fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BusId) REFERENCES Bus(BusId),
    FOREIGN KEY (ConductorId) REFERENCES Conductor(ConductorId)
);
CREATE TABLE Asiento (
    AsientoId INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(50) NOT NULL,
    Disponible BIT NOT NULL DEFAULT 1,
    RutaId INT NOT NULL,

    CONSTRAINT FK_Asiento_Ruta FOREIGN KEY (RutaId)
        REFERENCES Ruta(RutaId)
);
DECLARE @i INT = 1;
WHILE @i <= 20
BEGIN
    INSERT INTO Asiento (Codigo, Disponible, RutaId)
    VALUES (CONCAT('A', @i), 1, 1);
    SET @i = @i + 1;
END


------------------------------------------------------------
-- INSERTAR DATOS
------------------------------------------------------------

-- EMPRESAS
INSERT INTO Empresa (Nombre, Pais, Telefono) VALUES
('TransColombia', 'Colombia', '3001234567'),
('Rapido Bogota', 'Colombia', '3109876543'),
('AndesBus Colombia', 'Colombia', '3152244668'),
('SurAndino Colombia', 'Colombia', '3205566778'),
('LatinoTravel Colombia', 'Colombia', '3113344556');


-- BUSES
DECLARE @i INT = 1;
WHILE @i <= 20
BEGIN
    INSERT INTO Bus (EmpresaId, Placa, Modelo, Capacidad)
    VALUES (
        1 + ABS(CHECKSUM(NEWID())) % 5,
        CONCAT('BUS', @i),
        CONCAT('Modelo_', (ABS(CHECKSUM(NEWID())) % 5) + 1),
        40 + (ABS(CHECKSUM(NEWID())) % 20)
    );
    SET @i = @i + 1;
END;

-- CONDUCTORES
SET @i = 1;
WHILE @i <= 20
BEGIN
    INSERT INTO Conductor (Nombre, Licencia, Telefono)
    VALUES (
        CONCAT('Conductor_', @i),
        CONCAT('LIC', 1000 + @i),
        CONCAT('300', 1000000 + (@i * 23))
    );
    SET @i = @i + 1;
END;

-- RUTAS
INSERT INTO Ruta (Origen, Destino, DuracionMin, Precio, ImagenUrl, OrigenLat, OrigenLng, DestinoLat, DestinoLng) VALUES
('Bogota', 'Medellin', 480, 95000, 'https://picsum.photos/seed/bus/400', 4.7110, -74.0721, 6.2442, -75.5812),
('Bogota', 'Cali', 600, 110000, 'https://picsum.photos/seed/bus/400', 4.7110, -74.0721, 3.4516, -76.5320),
('Medellin', 'Cartagena', 720, 150000, 'https://picsum.photos/seed/bus/400', 6.2442, -75.5812, 10.3910, -75.4794),
('Cali', 'Pereira', 240, 60000, 'https://picsum.photos/seed/bus/400', 3.4516, -76.5320, 4.8146, -75.6946),
('Barranquilla', 'Santa Marta', 120, 40000, 'https://picsum.photos/seed/bus/400', 10.9685, -74.7813, 11.2408, -74.1990);



-- ITINERARIOS
SET @i = 1;
WHILE @i <= 40
BEGIN
    INSERT INTO Itinerario (RutaId, BusId, ConductorId, Fecha, HoraSalida, HoraLlegada)
    VALUES (
        1 + ABS(CHECKSUM(NEWID())) % 5,
        1 + ABS(CHECKSUM(NEWID())) % 20,
        1 + ABS(CHECKSUM(NEWID())) % 20,
        DATEADD(DAY, ABS(CHECKSUM(NEWID())) % 20, GETDATE()),
        '08:00',
        '12:00'
    );
    SET @i = @i + 1;
END;

-- USUARIOS
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO Usuario (Nombre, Email, Telefono)
    VALUES (
        CONCAT('Usuario_', @i),
        CONCAT('user', @i, '@correo.com'),
        CONCAT('311', 1000000 + (@i * 17))
    );
    SET @i = @i + 1;
END;

-- BOLETOS
SET @i = 1;
WHILE @i <= 80
BEGIN
    INSERT INTO Boleto (UsuarioId, ItinerarioId, FechaCompra, Asiento)
    VALUES (
        1 + ABS(CHECKSUM(NEWID())) % 50,
        1 + ABS(CHECKSUM(NEWID())) % 40,
        GETDATE(),
        CONCAT('A', (ABS(CHECKSUM(NEWID())) % 40) + 1)
    );
    SET @i = @i + 1;
END;

-- OFERTAS
INSERT INTO Oferta (EmpresaId, Titulo, Descripcion, Descuento) VALUES
(1, 'Dia sin IVA', 'Descuento especial', 20),
(3, 'Viaja en pareja', '2x1 en viajes seleccionados', 50),
(5, 'Promo fin de semana', 'Descuento para rutas largas', 30);

-- RESENAS
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO Resena (UsuarioId, RutaId, Calificacion, Comentario)
    VALUES (
        1 + ABS(CHECKSUM(NEWID())) % 50,
        1 + ABS(CHECKSUM(NEWID())) % 5,
        1 + ABS(CHECKSUM(NEWID())) % 5,
        CONCAT('Comentario del usuario ', @i)
    );
    SET @i = @i + 1;
END;

-- REPORTES
SET @i = 1;
WHILE @i <= 20
BEGIN
    INSERT INTO Reporte (BusId, ConductorId, Descripcion)
    VALUES (
        1 + ABS(CHECKSUM(NEWID())) % 20,
        1 + ABS(CHECKSUM(NEWID())) % 20,
        CONCAT('Reporte ', @i)
    );
    SET @i = @i + 1;
END;

SELECT 'BASE DE DATOS CREADA EXITOSAMENTE' AS Mensaje;

/*
--Vistas
CREATE VIEW VistaRutas AS
SELECT 
    RutaId,
    Origen,
    Destino,
    DuracionMin,
    Precio,
    ImagenUrl,
    OrigenLat,
    OrigenLng,
    DestinoLat,
    DestinoLng,
    Empresa
FROM dbo.Ruta;

*/


INSERT INTO Asiento (Codigo, RutaId)
VALUES 
('A1', 2),
('A2', 2),
('A3', 2),
('A4', 2),
('A5', 2),
('A6', 2),
('A7', 2),
('A8', 2),
('A9', 2),
('A10', 2);









