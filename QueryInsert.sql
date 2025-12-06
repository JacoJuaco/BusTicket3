------------------------------------------------------------
-- EMPRESAS
------------------------------------------------------------
INSERT INTO Empresa (Nombre, Pais, Telefono) VALUES
('Expreso Bolivariano', 'Colombia', '3001112233'),
('Copetran', 'Colombia', '3105556677');

------------------------------------------------------------
-- USUARIOS
------------------------------------------------------------
INSERT INTO Usuario (Nombre, Email, Telefono, Password) VALUES
('Juan Perez', 'juan@example.com', '3001234567', '123456'),
('Maria Lopez', 'maria@example.com', '3109876543', '123456');

------------------------------------------------------------
-- CONDUCTORES
------------------------------------------------------------
INSERT INTO Conductor (Nombre, Licencia, Telefono) VALUES
('Carlos Mendoza', 'CND-9988', '3112223344'),
('Jorge Torres', 'CND-5544', '3125557788');

------------------------------------------------------------
-- BUSES
------------------------------------------------------------
INSERT INTO Bus (Placa, Capacidad, Modelo, EmpresaId) VALUES
('ABC123', 40, 'Mercedes-Benz 2018', 1),
('XYZ987', 36, 'Chevrolet 2020', 2);

------------------------------------------------------------
-- RUTAS
------------------------------------------------------------
INSERT INTO Ruta (Origen, Destino, DuracionMin, Precio, ImagenUrl,
OrigenLat, OrigenLng, DestinoLat, DestinoLng, Empresa) VALUES
('Bogotá', 'Medellín', 480, 120000, 'imagen1.jpg',
 4.6097, -74.0817, 6.2442, -75.5812, 'Expreso Bolivariano'),

('Cali', 'Bogotá', 600, 150000, 'imagen2.jpg',
 3.4516, -76.5320, 4.6097, -74.0817, 'Copetran');

------------------------------------------------------------
-- ASIENTOS
------------------------------------------------------------
INSERT INTO Asiento (Codigo, Disponible, RutaId) VALUES
('A1', 1, 1),
('A2', 1, 1),
('A3', 0, 1),
('B1', 1, 2),
('B2', 0, 2);

------------------------------------------------------------
-- OFERTAS
------------------------------------------------------------
INSERT INTO Oferta (Titulo, Descripcion, Descuento,Vigente,EmpresaId) VALUES
('Descuento de Enero', '20% en rutas nacionales', 0.20,1,1),
('Promo Festiva', '15% en todos los destinos', 0.15,0,1);

------------------------------------------------------------
-- RESEÑAS
------------------------------------------------------------
INSERT INTO Resena (UsuarioId, RutaId, Calificacion, Comentario, Fecha) VALUES
(1, 1, 5, 'Excelente servicio', '2025-01-12'),
(2, 2, 4, 'Buen viaje, cómodo y seguro', '2025-01-18');

------------------------------------------------------------
-- VENTAS
------------------------------------------------------------
INSERT INTO Venta (AsientoId, RutaId, Fecha) VALUES
(1, 1, '2025-01-05'),
(5, 2, '2025-01-10');
