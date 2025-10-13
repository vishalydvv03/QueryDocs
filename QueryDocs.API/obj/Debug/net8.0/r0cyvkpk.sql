IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ChatLogs] (
    [Id] int NOT NULL IDENTITY,
    [Query] nvarchar(max) NOT NULL,
    [Response] nvarchar(max) NOT NULL,
    [ContextChunk] nvarchar(max) NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_ChatLogs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ExceptionLogs] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [StackTrace] nvarchar(max) NULL,
    [URL] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL,
    CONSTRAINT [PK_ExceptionLogs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [UserName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [ContactNo] nvarchar(10) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251011162046_InitialSetup', N'8.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [IsAdmin] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251013071036_AdminRole', N'8.0.20');
GO

COMMIT;
GO

