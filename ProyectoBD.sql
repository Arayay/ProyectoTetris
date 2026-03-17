IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'ProyectoFinalDb')
BEGIN
    CREATE DATABASE [ProyectoFinalDb];
END
GO

USE [ProyectoFinalDb];
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Users' AND type = 'U')
BEGIN
    CREATE TABLE dbo.Users (
        UserId INT IDENTITY(1,1) NOT NULL,
        Username NVARCHAR(50) NOT NULL,
        PasswordHash NVARCHAR(256) NOT NULL,
        Email NVARCHAR(100) NULL,
        MaxScore INT NOT NULL CONSTRAINT DF_Users_MaxScore DEFAULT (0),
        TotalGames INT NOT NULL CONSTRAINT DF_Users_TotalGames DEFAULT (0),
        IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_Users PRIMARY KEY (UserId),
        CONSTRAINT UQ_Users_Username UNIQUE (Username)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'GameSessions' AND type = 'U')
BEGIN
    CREATE TABLE dbo.GameSessions (
        GameSessionId INT IDENTITY(1,1) NOT NULL,
        UserId INT NOT NULL,
        Score INT NOT NULL CONSTRAINT DF_GameSessions_Score DEFAULT (0),
        Level INT NOT NULL CONSTRAINT DF_GameSessions_Level DEFAULT (1),
        LinesCleared INT NOT NULL CONSTRAINT DF_GameSessions_LinesCleared DEFAULT (0),
        DurationSeconds INT NOT NULL CONSTRAINT DF_GameSessions_DurationSeconds DEFAULT (0),
        StartedAt DATETIME2 NOT NULL CONSTRAINT DF_GameSessions_StartedAt DEFAULT (SYSUTCDATETIME()),
        EndedAt DATETIME2 NULL,
        CONSTRAINT PK_GameSessions PRIMARY KEY (GameSessionId),
        CONSTRAINT FK_GameSessions_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users(UserId)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UserSettings' AND type = 'U')
BEGIN
    CREATE TABLE dbo.UserSettings (
        UserSettingsId INT IDENTITY(1,1) NOT NULL,
        UserId INT NOT NULL,
        BoardWidth INT NOT NULL CONSTRAINT DF_UserSettings_BoardWidth DEFAULT (10),
        BoardHeight INT NOT NULL CONSTRAINT DF_UserSettings_BoardHeight DEFAULT (20),
        StartingLevel INT NOT NULL CONSTRAINT DF_UserSettings_StartingLevel DEFAULT (1),
        Theme NVARCHAR(20) NOT NULL CONSTRAINT DF_UserSettings_Theme DEFAULT ('Default'),
        IsSoundEnabled BIT NOT NULL CONSTRAINT DF_UserSettings_IsSoundEnabled DEFAULT (1),
        CONSTRAINT PK_UserSettings PRIMARY KEY (UserSettingsId),
        CONSTRAINT FK_UserSettings_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users(UserId),
        CONSTRAINT UQ_UserSettings_UserId UNIQUE (UserId)
    );
END
GO

IF COL_LENGTH('dbo.Users', 'Role') IS NULL
BEGIN
    ALTER TABLE dbo.Users
    ADD Role NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Users_Role DEFAULT ('Player');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'admin')
BEGIN
    INSERT INTO dbo.Users (Username, PasswordHash, Email, MaxScore, TotalGames, IsActive)
    VALUES ('admin', 'admin123', 'admin@proyectofinal.com', 0, 0, 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.UserSettings us
               INNER JOIN dbo.Users u ON us.UserId = u.UserId
               WHERE u.Username = 'admin')
BEGIN
    INSERT INTO dbo.UserSettings (UserId, BoardWidth, BoardHeight, StartingLevel, Theme, IsSoundEnabled)
    SELECT UserId, 10, 20, 1, 'Default', 1
    FROM dbo.Users
    WHERE Username = 'admin';
END
GO


USE [ProyectoFinalDb];
GO

SELECT * FROM dbo.Users;




USE [ProyectoFinalDb];
GO


UPDATE dbo.Users
SET Role = 'Admin'
WHERE Username = 'root';
GO
