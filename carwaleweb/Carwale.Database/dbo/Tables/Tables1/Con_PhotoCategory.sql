CREATE TABLE [dbo].[Con_PhotoCategory] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [MainCategory]  TINYINT       NULL,
    [ApplicationId] TINYINT       NULL,
    [BWMigratedId]  INT           NULL,
    CONSTRAINT [PK_Con_PhotoCategory] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

