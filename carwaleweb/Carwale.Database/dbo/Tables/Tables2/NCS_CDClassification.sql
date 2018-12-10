CREATE TABLE [dbo].[NCS_CDClassification] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]      VARCHAR (100) NULL,
    [CarMakeId] NUMERIC (18)  NULL,
    [IsActive]  BIT           CONSTRAINT [DF_NCS_CCName_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_NCS_ClassificationName] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

