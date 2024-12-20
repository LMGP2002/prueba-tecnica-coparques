USE [SistemaReservas]
GO
/****** Object:  Table [dbo].[Reservas]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SalaID] [int] NULL,
	[FechaReserva] [date] NOT NULL,
	[Usuario] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Salas]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Salas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Capacidad] [int] NOT NULL,
	[Disponibilidad] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD FOREIGN KEY([SalaID])
REFERENCES [dbo].[Salas] ([ID])
GO
/****** Object:  StoredProcedure [dbo].[spAgregarReserva]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAgregarReserva]
    @SalaID INT, 
    @FechaReserva DATE, 
    @Usuario NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM Reservas WHERE SalaID = @SalaID AND FechaReserva = @FechaReserva
    )
    BEGIN
        INSERT INTO Reservas (SalaID, FechaReserva, Usuario)
        VALUES (@SalaID, @FechaReserva, @Usuario);
    END
    ELSE
    BEGIN
        THROW 50001, 'La sala ya está reservada para esa fecha.', 1;
    END;
END;
GO
/****** Object:  StoredProcedure [dbo].[spAgregarSala]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAgregarSala]
    @Nombre NVARCHAR(100), 
    @Capacidad INT, 
    @Disponibilidad BIT
AS
BEGIN
    INSERT INTO Salas (Nombre, Capacidad, Disponibilidad)
    VALUES (@Nombre, @Capacidad, @Disponibilidad);
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultarReservas]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spConsultarReservas]
AS
BEGIN
    SELECT 
        r.ID,
        r.SalaID,
        r.FechaReserva,
        r.Usuario,
        s.Nombre AS NombreSala
    FROM Reservas r
    INNER JOIN Salas s ON r.SalaID = s.ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultarSalas]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spConsultarSalas]
    @Estado INT = NULL  -- Cambiar el tipo de estado a INT
AS
BEGIN
    -- Si el parámetro @Estado es NULL, devuelve todas las salas
    -- Si el parámetro tiene un valor, filtra por ese estado (0 o 1)
    IF @Estado IS NULL
    BEGIN
        SELECT * FROM Salas;
    END
    ELSE
    BEGIN
        SELECT * 
        FROM Salas 
        WHERE Disponibilidad = @Estado;
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditarReserva]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEditarReserva]
    @ID INT,
    @SalaID INT,
    @FechaReserva DATE,
    @Usuario NVARCHAR(100)
AS
BEGIN
    UPDATE Reservas
    SET SalaID = @SalaID,
        FechaReserva = @FechaReserva,
        Usuario = @Usuario
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEditarSala]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spEditarSala]
    @ID INT, 
    @Nombre NVARCHAR(100), 
    @Capacidad INT, 
    @Disponibilidad BIT
AS
BEGIN
    UPDATE Salas
    SET Nombre = @Nombre, Capacidad = @Capacidad, Disponibilidad = @Disponibilidad
    WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEliminarReserva]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEliminarReserva]
    @ID INT
AS
BEGIN
    DELETE FROM Reservas WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEliminarSala]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spEliminarSala]
    @ID INT
AS
BEGIN
    DELETE FROM Salas WHERE ID = @ID;
END;
GO
/****** Object:  StoredProcedure [dbo].[spFiltrarReservas]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spFiltrarReservas]
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL,
    @SalaID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.ID,
        r.SalaID,
        r.FechaReserva,
        r.Usuario,
        s.Nombre AS NombreSala
    FROM Reservas r
    INNER JOIN Salas s ON r.SalaID = s.ID
    WHERE 
        (@FechaInicio IS NULL OR r.FechaReserva >= @FechaInicio) AND
        (@FechaFin IS NULL OR r.FechaReserva <= @FechaFin) AND
        (@SalaID IS NULL OR r.SalaID = @SalaID)
    ORDER BY r.FechaReserva;
END;
GO
/****** Object:  StoredProcedure [dbo].[spTieneReservas]    Script Date: 12/18/2024 11:45:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTieneReservas]
    @SalaID INT
AS
BEGIN
    SELECT CASE 
        WHEN EXISTS (SELECT 1 FROM Reservas WHERE SalaID = @SalaID) THEN 1 
        ELSE 0 
    END
END
GO
