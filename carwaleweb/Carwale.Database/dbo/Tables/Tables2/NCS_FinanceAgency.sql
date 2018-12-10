CREATE TABLE [dbo].[NCS_FinanceAgency] (
    [Id]           NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]         VARCHAR (200)   NOT NULL,
    [IsActive]     BIT             CONSTRAINT [DF_NCS_FinanceAgency_IsActive] DEFAULT ((1)) NOT NULL,
    [TDS]          DECIMAL (18, 2) NULL,
    [IRType]       SMALLINT        NULL,
    [ApprovalTime] INT             NULL,
    [LastUpdated]  DATETIME        NULL,
    CONSTRAINT [PK_NCS_FinanceAgency] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Fixed, 2-Floating', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_FinanceAgency', @level2type = N'COLUMN', @level2name = N'IRType';

