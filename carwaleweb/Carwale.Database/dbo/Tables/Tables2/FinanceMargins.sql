CREATE TABLE [dbo].[FinanceMargins] (
    [ID]             INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FinancerId]     INT             NOT NULL,
    [Tenure]         INT             NOT NULL,
    [Margin]         DECIMAL (10, 2) NOT NULL,
    [CustomerMargin] DECIMAL (10, 2) NOT NULL,
    CONSTRAINT [PK_FinanceMargins] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

