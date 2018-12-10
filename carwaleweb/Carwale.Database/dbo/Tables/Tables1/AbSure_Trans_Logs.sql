CREATE TABLE [dbo].[AbSure_Trans_Logs] (
    [ID]                NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [TransactionType]   TINYINT         NOT NULL,
    [TransactionId]     NUMERIC (18)    NOT NULL,
    [TransactionAmount] NUMERIC (18, 2) NULL,
    [ClosingAmount]     NUMERIC (18)    NOT NULL,
    [LogDate]           DATETIME        CONSTRAINT [DF_AbSure_Trans_Logs_LogDate] DEFAULT (getdate()) NOT NULL,
    [LoggedBy]          INT             NOT NULL,
    CONSTRAINT [PK_AbSure_Trans_Logs] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Credit, 2-Debit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AbSure_Trans_Logs', @level2type = N'COLUMN', @level2name = N'TransactionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AbSure_Trans_Credits Id or AbSure_Trans_Debits Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AbSure_Trans_Logs', @level2type = N'COLUMN', @level2name = N'TransactionId';

