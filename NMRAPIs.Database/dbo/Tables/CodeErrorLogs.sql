CREATE TABLE [dbo].[CodeErrorLogs] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Method]          NVARCHAR (MAX) NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [StackTrace]      NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [Arguments]       NVARCHAR (MAX) NULL,
    [ProfileId]       INT            NULL,
    [StoredProcedure] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

