CREATE TABLE [dbo].[OLM_SCPartsMaster] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [PartDescription] VARCHAR (50) NOT NULL,
    [PartNumber]      VARCHAR (50) NULL,
    [PartType]        SMALLINT     NOT NULL,
    CONSTRAINT [PK_OLM_SCPartsMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

