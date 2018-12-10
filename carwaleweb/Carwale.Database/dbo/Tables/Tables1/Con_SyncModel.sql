CREATE TABLE [dbo].[Con_SyncModel] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]      NUMERIC (18) NOT NULL,
    [ModelName]    VARCHAR (30) NOT NULL,
    [CreatedOn]    DATETIME     NOT NULL,
    [CreatedBy]    NUMERIC (18) NOT NULL,
    [ServerName]   VARCHAR (30) NOT NULL,
    [IsReplicated] BIT          NULL,
    CONSTRAINT [PK_Con_SyncModel_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

