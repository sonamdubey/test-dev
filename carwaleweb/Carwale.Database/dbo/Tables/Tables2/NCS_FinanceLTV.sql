CREATE TABLE [dbo].[NCS_FinanceLTV] (
    [Id]              NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FinanceAgencyId] NUMERIC (18)    NULL,
    [ModelId]         NUMERIC (18)    NULL,
    [VersionId]       NUMERIC (18)    CONSTRAINT [DF_NCS_FinanceLTV_VersionId] DEFAULT ((-1)) NOT NULL,
    [StartTenure]     INT             NULL,
    [EndTenure]       INT             NULL,
    [Value]           NUMERIC (5, 2)  NULL,
    [Tag]             VARCHAR (100)   NULL,
    [LastUpdated]     DATETIME        NULL,
    [StartIncome]     DECIMAL (18, 2) NULL,
    [EndIncome]       DECIMAL (18, 2) NULL,
    [IsSalaried]      BIT             NULL,
    CONSTRAINT [PK_NCS_FinanceLTV] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

