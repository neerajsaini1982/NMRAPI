CREATE TABLE [dbo].[Profiles] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]     NVARCHAR (1000) NOT NULL,
    [LastName]      NVARCHAR (1000) NOT NULL,
    [UserName]      NVARCHAR (1000) NULL,
    [Email ]        NVARCHAR (1000) NULL,
    [Password]      NVARCHAR (MAX)  NULL,
    [IsDeleted]     BIT             NOT NULL DEFAULT 0,
    [CreatedAt]     DATETIME2 (7)   NULL,
    [CreatedBy]     INT             NULL,
    [LastUpdatedAt] DATETIME2 (7)   NULL,
    [LastUpdatedBy] INT             NULL
);

