-- =============================================
-- Script de creacion de Base de Datos
-- Prueba Tecnica - Lockton Colombia
-- Candidata: Bibiana Sepulveda
-- Fecha: 19/10/2026
-- Se puede ejecutar multiples veces sin error
-- =============================================

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PruebaTecnicaLocktonDb')
BEGIN
    CREATE DATABASE PruebaTecnicaLocktonDb;
    PRINT 'Base de datos creada.';
END
ELSE
    PRINT 'Base de datos ya existe.';
GO

USE PruebaTecnicaLocktonDb;
GO

-- Tabla Usuarios
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Nombre NVARCHAR(200) NOT NULL,
        Correo NVARCHAR(450) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    CREATE UNIQUE INDEX IX_Usuarios_Correo ON Usuarios(Correo);
    PRINT 'Tabla Usuarios creada.';
END
ELSE
    PRINT 'Tabla Usuarios ya existe.';
GO

-- Tabla Menus
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Menus')
BEGIN
    CREATE TABLE Menus (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Nombre NVARCHAR(200) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        Ruta NVARCHAR(200) NOT NULL,
        Activo BIT NOT NULL DEFAULT 1
    );
    PRINT 'Tabla Menus creada.';
END
ELSE
    PRINT 'Tabla Menus ya existe.';
GO

-- Tabla intermedia UsuarioMenus
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UsuarioMenus')
BEGIN
    CREATE TABLE UsuarioMenus (
        UsuarioId UNIQUEIDENTIFIER NOT NULL,
        MenuId UNIQUEIDENTIFIER NOT NULL,
        CONSTRAINT PK_UsuarioMenus PRIMARY KEY (UsuarioId, MenuId),
        CONSTRAINT FK_UsuarioMenus_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE,
        CONSTRAINT FK_UsuarioMenus_Menus FOREIGN KEY (MenuId) REFERENCES Menus(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_UsuarioMenus_MenuId ON UsuarioMenus(MenuId);
    PRINT 'Tabla UsuarioMenus creada.';
END
ELSE
    PRINT 'Tabla UsuarioMenus ya existe.';
GO

-- =============================================
-- Datos iniciales - 4 Menus (solo si no existen)
-- =============================================
IF NOT EXISTS (SELECT * FROM Menus WHERE Id = '11111111-1111-1111-1111-111111111111')
    INSERT INTO Menus (Id, Nombre, Descripcion, Ruta, Activo) VALUES ('11111111-1111-1111-1111-111111111111', 'Dashboard', 'Panel principal', '/dashboard', 1);

IF NOT EXISTS (SELECT * FROM Menus WHERE Id = '22222222-2222-2222-2222-222222222222')
    INSERT INTO Menus (Id, Nombre, Descripcion, Ruta, Activo) VALUES ('22222222-2222-2222-2222-222222222222', 'Usuarios', 'Administracion de usuarios', '/usuarios', 1);

IF NOT EXISTS (SELECT * FROM Menus WHERE Id = '33333333-3333-3333-3333-333333333333')
    INSERT INTO Menus (Id, Nombre, Descripcion, Ruta, Activo) VALUES ('33333333-3333-3333-3333-333333333333', 'Reportes', 'Modulo de reportes', '/reportes', 1);

IF NOT EXISTS (SELECT * FROM Menus WHERE Id = '44444444-4444-4444-4444-444444444444')
    INSERT INTO Menus (Id, Nombre, Descripcion, Ruta, Activo) VALUES ('44444444-4444-4444-4444-444444444444', 'Configuracion', 'Configuracion del sistema', '/configuracion', 1);

PRINT 'Datos iniciales de menus verificados.';
GO

GO
