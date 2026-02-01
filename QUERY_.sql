CREATE DATABASE db_ac4a69_ordermanagement;
GO
USE db_ac4a69_ordermanagement;
GO
CREATE TABLE Cliente (
ClienteId BIGINT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(100) NOT NULL,
Identidad NVARCHAR(50) NOT NULL UNIQUE,
CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE Producto (
ProductoId BIGINT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(100) NOT NULL,
Descripcion NVARCHAR(500),
Precio DECIMAL(10,2) NOT NULL,
Existencia INT NOT NULL DEFAULT 0,
CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE Orden (
OrdenId BIGINT IDENTITY(1,1) PRIMARY KEY,
ClienteId BIGINT NOT NULL,
Impuesto DECIMAL(10,2) NOT NULL DEFAULT 0,
Subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
Total DECIMAL(10,2) NOT NULL DEFAULT 0,
FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),

CONSTRAINT FK_Orden_Cliente FOREIGN KEY (ClienteId) REFERENCES
Cliente(ClienteId)
);
CREATE TABLE DetalleOrden (
DetalleOrdenId BIGINT IDENTITY(1,1) PRIMARY KEY,
OrdenId BIGINT NOT NULL,
ProductoId BIGINT NOT NULL,
Cantidad INT NOT NULL,
Impuesto DECIMAL(10,2) NOT NULL,
Subtotal DECIMAL(10,2) NOT NULL,
Total DECIMAL(10,2) NOT NULL,
CONSTRAINT FK_DetalleOrden_Orden FOREIGN KEY (OrdenId) REFERENCES
Orden(OrdenId),
CONSTRAINT FK_DetalleOrden_Producto FOREIGN KEY (ProductoId) REFERENCES
Producto(ProductoId)
);
-- Índices para performance
CREATE INDEX IX_Orden_ClienteId ON Orden(ClienteId);
CREATE INDEX IX_DetalleOrden_OrdenId ON DetalleOrden(OrdenId);
CREATE INDEX IX_DetalleOrden_ProductoId ON DetalleOrden(ProductoId);
-- Datos de prueba
INSERT INTO Cliente (Nombre, Identidad) VALUES
('Juan Pérez', '0801-1990-12345'),
('María González', '0801-1985-67890'),
('Carlos Rodríguez', '0801-1992-11111');
INSERT INTO Producto (Nombre, Descripcion, Precio, Existencia) VALUES
('Laptop Dell XPS 15', 'Laptop de alto rendimiento', 1299.99, 50),
('Mouse Logitech MX Master 3', 'Mouse ergonómico inalámbrico', 99.99, 150),
('Teclado Mecánico Keychron K2', 'Teclado mecánico retroiluminado', 89.99, 75),
('Monitor LG 27" 4K', 'Monitor 4K UHD', 449.99, 30),
('Webcam Logitech C920', 'Webcam Full HD 1080p', 79.99, 100);




-- Listar Clientes
CREATE OR ALTER PROCEDURE sp_ObtenerClientes
AS
BEGIN
    SELECT ClienteId, Nombre, Identidad,CreatedAt FROM Cliente;
END;
GO

-- Obtener Cliente por ID
CREATE OR ALTER PROCEDURE sp_ObtenerClientesPorId
    @ClienteId BIGINT
AS
BEGIN
    SELECT ClienteId, Nombre, Identidad,CreatedAt FROM Cliente WHERE ClienteId = @ClienteId;
END;
GO

-- Crear Cliente 
CREATE OR ALTER PROCEDURE sp_InsertarCliente
    @Nombre NVARCHAR(100),
    @Identidad NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Cliente WHERE Identidad = @Identidad)
        BEGIN
            SELECT -2 AS Resultado; 
            RETURN;
        END

        INSERT INTO Cliente (Nombre, Identidad)
        VALUES (@Nombre, @Identidad);

        SELECT SCOPE_IDENTITY() AS Resultado; 
    END TRY
    BEGIN CATCH
        SELECT -1 AS Resultado; 
    END CATCH
END;
GO
--actualizar cliente

CREATE OR ALTER PROCEDURE sp_ActualizarCliente
    @ClienteId BIGINT,
    @Nombre NVARCHAR(100),
    @Identidad NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Cliente WHERE ClienteId = @ClienteId)
        BEGIN
            SELECT 0 AS Resultado;
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Cliente WHERE Identidad = @Identidad AND ClienteId <> @ClienteId)
        BEGIN
            SELECT -2 AS Resultado; 
            RETURN;
        END

        UPDATE Cliente
        SET Nombre = @Nombre,
            Identidad = @Identidad
        WHERE ClienteId = @ClienteId;

        SELECT 1 AS Resultado; 
    END TRY
    BEGIN CATCH
        SELECT -1 AS Resultado;
    END CATCH
END;
GO
-- Productos
-- Listar Productos
CREATE OR ALTER PROCEDURE sp_ObtenerProductos
AS
BEGIN
    SELECT ProductoId, Nombre, Descripcion, Precio, Existencia,CreatedAt FROM Producto;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerProductosPorID
@ProductoId BIGINT
AS
BEGIN
    SELECT ProductoId, Nombre, Descripcion, Precio, Existencia,CreatedAt FROM Producto Where ProductoId = @ProductoId;
END;
GO

-- crear producto
CREATE OR ALTER PROCEDURE sp_InsertarProducto
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(500),
    @Precio DECIMAL(10,2),
    @Existencia INT
AS
BEGIN
    BEGIN TRY
        
        IF (@Precio <= 0 OR @Existencia < 0)
        BEGIN
            SELECT -2 AS Resultado; 
            RETURN;
        END

        INSERT INTO Producto (Nombre, Descripcion, Precio, Existencia)
        VALUES (@Nombre, @Descripcion, @Precio, @Existencia);

        SELECT SCOPE_IDENTITY() AS Resultado; 
    END TRY
    BEGIN CATCH
        SELECT -1 AS Resultado; 
    END CATCH
END;
GO
-- Actualizar Producto 

CREATE OR ALTER PROCEDURE sp_ActualizarProducto
    @ProductoId BIGINT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(500),
    @Precio DECIMAL(10,2),
    @Existencia INT
AS
BEGIN
    BEGIN TRY
       
        IF NOT EXISTS (SELECT 1 FROM Producto WHERE ProductoId = @ProductoId)
        BEGIN
            SELECT 0 AS Resultado; 
            RETURN;
        END

        
        IF (@Precio <= 0 OR @Existencia < 0)
        BEGIN
            SELECT -2 AS Resultado;
            RETURN;
        END

        
        UPDATE Producto 
        SET Nombre = @Nombre, 
            Descripcion = @Descripcion, 
            Precio = @Precio, 
            Existencia = @Existencia
        WHERE ProductoId = @ProductoId;

        SELECT 1 AS Resultado; 
    END TRY
    BEGIN CATCH
        SELECT -1 AS Resultado; 
    END CATCH
END;
GO

--ORDEN
--crear orden
CREATE PROCEDURE dbo.sp_CreateOrden
    @ClienteId BIGINT,
    @Detalles XML
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @OrdenId BIGINT;

    BEGIN TRY
        BEGIN TRANSACTION;

     
        IF NOT EXISTS (SELECT 1 FROM dbo.Cliente WHERE ClienteId = @ClienteId)
        BEGIN
            THROW 50001, 'Cliente no existe.', 1;
        END;


        INSERT INTO dbo.Orden (ClienteId, Impuesto, Subtotal, Total)
        VALUES (@ClienteId, 0, 0, 0);
        SET @OrdenId = CAST(SCOPE_IDENTITY() AS BIGINT);

   
        INSERT INTO dbo.DetalleOrden (OrdenId, ProductoId, Cantidad, Subtotal, Impuesto, Total)
        SELECT
            @OrdenId,
            x.value('(ProductoId)[1]', 'BIGINT') AS ProductoId,
            x.value('(Cantidad)[1]', 'INT') AS Cantidad,
            (p.Precio * x.value('(Cantidad)[1]', 'INT')) AS Subtotal,
            (p.Precio * x.value('(Cantidad)[1]', 'INT') * 0.15) AS Impuesto,
            (p.Precio * x.value('(Cantidad)[1]', 'INT') * 1.15) AS Total
        FROM @Detalles.nodes('/Detalles/Detalle') AS T(x)
        JOIN dbo.Producto p ON p.ProductoId = x.value('(ProductoId)[1]', 'BIGINT');

  
        IF EXISTS (
            SELECT 1
            FROM dbo.DetalleOrden d
            JOIN dbo.Producto p ON d.ProductoId = p.ProductoId
            WHERE d.OrdenId = @OrdenId AND p.Existencia < d.Cantidad
        )
        BEGIN
            THROW 50002, 'Existencias insuficientes para uno o mas productos.', 1;
        END;

      
        UPDATE p
        SET p.Existencia = p.Existencia - d.Cantidad
        FROM dbo.Producto p
        INNER JOIN dbo.DetalleOrden d ON p.ProductoId = d.ProductoId
        WHERE d.OrdenId = @OrdenId;

      
        UPDATE dbo.Orden
        SET Subtotal = (SELECT SUM(Subtotal) FROM dbo.DetalleOrden WHERE OrdenId = @OrdenId),
            Impuesto  = (SELECT SUM(Impuesto)  FROM dbo.DetalleOrden WHERE OrdenId = @OrdenId),
            Total     = (SELECT SUM(Total)     FROM dbo.DetalleOrden WHERE OrdenId = @OrdenId)
        WHERE OrdenId = @OrdenId;

        COMMIT TRANSACTION;

        SELECT @OrdenId AS NewOrdenId;

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

SELECT * from Producto


DECLARE @xmlData XML;
SET @xmlData = '
<Detalles>
    <Detalle>
        <ProductoId>1</ProductoId>
        <Cantidad>2</Cantidad>
    </Detalle>
    <Detalle>
        <ProductoId>2</ProductoId>
        <Cantidad>1</Cantidad>
    </Detalle>
</Detalles>';

EXEC dbo.sp_CreateOrden @ClienteId = 1, @Detalles = @xmlData;

SELECT * FROM Orden ORDER BY OrdenId DESC;
SELECT * FROM DetalleOrden WHERE OrdenId = (SELECT MAX(OrdenId) FROM Orden);