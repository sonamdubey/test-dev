CREATE TABLE [dbo].[OLM_SCReplacementParts] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [PartId]           NUMERIC (18) NOT NULL,
    [VersionId]        INT          NOT NULL,
    [PartCost]         INT          NOT NULL,
    [LabourPercentage] INT          NOT NULL,
    CONSTRAINT [PK_OLM_SCReplacementParts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

