CREATE TABLE [dbo].[ModelTopVersionLogs] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [SPName]       VARCHAR (100) NULL,
    [CarModelId]   INT           NULL,
    [carVersionId] INT           NULL,
    [ExecutedOn]   DATETIME      CONSTRAINT [DF_ModelTopVersionLogs_ExecutedOn] DEFAULT (getdate()) NULL
);

