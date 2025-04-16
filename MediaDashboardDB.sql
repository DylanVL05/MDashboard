CREATE DATABASE MediaDashboard;
GO
USE MediaDashboard;
GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL, 
    Rol NVARCHAR(50) DEFAULT 'Usuario',  -- Puede ser 'Admin', 'Usuario'
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- Tabla Components (para clasificar entre Widget, Chart, Table, etc.)
CREATE TABLE Components (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Tipo NVARCHAR(50) NOT NULL,  -- Ejemplo: 'Widget', 'Chart', 'Table'
    Descripcion NVARCHAR(255)
);
GO

-- Tabla Widgets (modificada para usar la nueva tabla Components)
CREATE TABLE Widgets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    ComponentId INT NOT NULL,  -- Refencia a la tabla Components
    URL_API NVARCHAR(500) NOT NULL, -- URL de la API de donde obtiene datos
    API_Key NVARCHAR(255) NULL,  -- Opcional si requiere API Key
    EsPublico BIT DEFAULT 1, -- 1 = Disponible para todos, 0 = Privado
    Estado BIT DEFAULT 1, -- 1 = Activo, 0 = Inactivo
    FOREIGN KEY (ComponentId) REFERENCES Components(Id) ON DELETE CASCADE
);
GO

-- Tabla ConfiguracionWidgets (modificada para usar la tabla Components)
CREATE TABLE ConfiguracionWidgets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    WidgetId INT NOT NULL,
    Posicion INT DEFAULT 0, -- Posición en la pantalla
    Tamano NVARCHAR(50) DEFAULT 'medium', -- small, medium, large
    RefrescoSegundos INT DEFAULT 60, -- Cada cuánto se actualiza
    EsFavorito BIT DEFAULT 0, -- 1 = Se muestra arriba, 0 = Normal
    EsVisible BIT DEFAULT 1, -- 1 = Visible, 0 = Oculto
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE,
    FOREIGN KEY (WidgetId) REFERENCES Widgets(Id) ON DELETE CASCADE
);
GO

-- Tabla PermisosWidgets (modificada para usar la tabla Components)
CREATE TABLE PermisosWidgets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    WidgetId INT NOT NULL,
    PuedeVer BIT DEFAULT 1, -- 1 = Puede ver, 0 = No puede ver
    PuedeEditar BIT DEFAULT 0, -- 1 = Puede modificar
    PuedeEliminar BIT DEFAULT 0, -- 1 = Puede eliminar
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE,
    FOREIGN KEY (WidgetId) REFERENCES Widgets(Id) ON DELETE CASCADE
);
GO

-- Tabla Logs (sin cambios)
CREATE TABLE Logs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Accion NVARCHAR(255) NOT NULL, -- Ejemplo: 'Añadió widget', 'Modificó configuración'
    Fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE
);
GO
