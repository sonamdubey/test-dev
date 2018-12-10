CREATE TABLE [dbo].[WA_Keys] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [SourceId] INT          NULL,
    [CWK]      VARCHAR (30) NULL,
    [IsActive] BIT          NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

