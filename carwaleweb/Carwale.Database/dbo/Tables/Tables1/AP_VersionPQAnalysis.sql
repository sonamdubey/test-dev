CREATE TABLE [dbo].[AP_VersionPQAnalysis] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId]  NUMERIC (18)  NULL,
    [PQCarVersions] VARCHAR (100) NULL,
    [LastUpdated]   DATETIME      NULL,
    CONSTRAINT [PK_AP_VersionPQAnalysis] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

