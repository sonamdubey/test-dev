CREATE TABLE [dbo].[TC_EWalletTransactions] (
    [TransactionId]       INT      IDENTITY (22, 1) NOT NULL,
    [TC_RedeemedPointsId] INT      NULL,
    [ApprovedBy]          INT      NULL,
    [ApprovedOn]          DATETIME NULL,
    PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);

