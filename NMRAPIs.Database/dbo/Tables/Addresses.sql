CREATE TABLE [dbo].[Addresses] (
    [AddressId]            INT             IDENTITY (1, 1) NOT NULL,
    [AddressLine]     NVARCHAR (500) NOT NULL,
    [City]      NVARCHAR (200) NOT NULL,
    [State]      NVARCHAR (200) NOT NULL,
    [Zip]        NVARCHAR (100) NOT NULL,
    [CreatedAt]     DATETIME2 (7)   NULL,
    [CreatedBy]     INT             NULL,
    [LastUpdatedAt] DATETIME2 (7)   NULL,
    [LastUpdatedBy] INT             NULL
);

