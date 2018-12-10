CREATE TABLE [dbo].[ICB_FinanceAgency] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]         VARCHAR (200)  NOT NULL,
    [IsActive]     BIT            NOT NULL,
    [TDS]          DECIMAL (5, 2) NULL,
    [IRType]       SMALLINT       NULL,
    [ApprovalTime] INT            NULL,
    [LastUpdated]  DATETIME       NULL,
    CONSTRAINT [PK_ICB_FinanceAgency] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

