CREATE TABLE [dbo].[TC_RedeemedPointsLog] (
    [TC_RedeemedPointsLogId] INT           IDENTITY (1, 1) NOT NULL,
    [Comment]                VARCHAR (300) NULL,
    [RequestType]            INT           NULL,
    [ActionTakenOn]          DATETIME      NULL,
    [ActionTakenBy]          INT           NULL,
    [TC_RedeemedPointsId]    INT           NULL,
    [PayUMoneyTransactionId] INT           NULL,
    [PayUMoneyStatus]        BIT           NULL,
    [PayUMoneyMessage]       VARCHAR (500) NULL
);

