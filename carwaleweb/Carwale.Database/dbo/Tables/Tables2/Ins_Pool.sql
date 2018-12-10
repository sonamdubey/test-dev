CREATE TABLE [dbo].[Ins_Pool] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]     NUMERIC (18) NOT NULL,
    [VersionId]      NUMERIC (18) NOT NULL,
    [MakeYear]       DATETIME     NULL,
    [RegNo]          VARCHAR (50) NULL,
    [InsuranceExp]   DATETIME     NULL,
    [EntryDateTime]  DATETIME     NOT NULL,
    [UpdateDateTime] DATETIME     NULL,
    [SourceId]       INT          NULL,
    [IsActive]       BIT          CONSTRAINT [DF_Ins_Pool_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Ins_Pool] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for MyGarage, 2 for INS_PremiumLeads and 3 for unknown source', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Ins_Pool', @level2type = N'COLUMN', @level2name = N'SourceId';

