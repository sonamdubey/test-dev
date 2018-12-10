CREATE TABLE [dbo].[FinanceDocuments] (
    [Id]                INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]              VARCHAR (200) NOT NULL,
    [FinanceCategoryId] INT           NULL,
    [IsActive]          BIT           CONSTRAINT [DF_FinanceDocuments_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_FinanceDocuments] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

