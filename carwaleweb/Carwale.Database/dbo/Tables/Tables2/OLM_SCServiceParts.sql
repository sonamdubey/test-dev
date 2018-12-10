CREATE TABLE [dbo].[OLM_SCServiceParts] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [PartId]    NUMERIC (18) NOT NULL,
    [VersionId] INT          NOT NULL,
    [Price]     INT          NULL,
    [Quantity]  VARCHAR (10) NULL,
    [15K]       INT          NULL,
    [30K]       INT          NULL,
    [45K]       INT          NULL,
    [60K]       INT          NULL,
    [75K]       INT          NULL,
    [90K]       INT          NULL,
    [105K]      INT          NULL,
    [120K]      INT          NULL,
    [135K]      INT          NULL,
    [150K]      INT          NULL,
    CONSTRAINT [PK_OLM_SCServiceParts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

