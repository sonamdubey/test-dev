CREATE TABLE [dbo].[TC_RedeemedPoints] (
    [Id]                     INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]               INT           NULL,
    [RedeemDate]             DATETIME      NULL,
    [Denomination]           VARCHAR (10)  NULL,
    [Quantity]               SMALLINT      NULL,
    [RedeemedPoints]         NUMERIC (18)  NULL,
    [Description]            VARCHAR (50)  NULL,
    [EmailSentOn]            VARCHAR (50)  NULL,
    [RequestType]            INT           DEFAULT ((1)) NULL,
    [ApprovalDate]           DATETIME      NULL,
    [SentDate]               DATETIME      NULL,
    [PIN]                    VARCHAR (100) NULL,
    [VoucherNo]              VARCHAR (250) NULL,
    [ExpiryDate]             DATETIME      NULL,
    [UserId]                 INT           NULL,
    [TC_EWalletsId]          INT           DEFAULT ((1)) NULL,
    [PayUMoneyTransactionId] INT           NULL,
    [PayUMoneyStatus]        BIT           NULL,
    [PayUMoneyMessage]       VARCHAR (500) NULL,
    [RedeemedAmount]         INT           NULL
);

