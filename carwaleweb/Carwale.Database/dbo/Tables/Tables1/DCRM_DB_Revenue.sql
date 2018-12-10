CREATE TABLE [dbo].[DCRM_DB_Revenue] (
    [Id]                NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [DealerId]          NUMERIC (18)    NOT NULL,
    [ContractId]        NUMERIC (18)    NULL,
    [UserId]            INT             NULL,
    [BusinessUnitId]    INT             NULL,
    [StartDate]         DATETIME        NULL,
    [EndDate]           DATETIME        NULL,
    [TotalDelivered]    INT             NULL,
    [CostPerUnit]       NUMERIC (18, 2) NULL,
    [ContractBehaviour] SMALLINT        NOT NULL,
    [EntryDate]         DATETIME        CONSTRAINT [DF_DCRM_DB_Revenue_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DCRM_DB_Revenue] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SalesExecutiveId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_DB_Revenue', @level2type = N'COLUMN', @level2name = N'UserId';

