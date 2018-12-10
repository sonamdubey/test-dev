CREATE TABLE [dbo].[TC_RedeemedPointsBW310816_bkp] (
    [Id]                     INT           NOT NULL,
    [DealerId]               INT           NULL,
    [RedeemDate]             DATETIME      NULL,
    [Denomination]           VARCHAR (10)  NULL,
    [Quantity]               SMALLINT      NULL,
    [RedeemedPoints]         NUMERIC (18)  NULL,
    [Description]            VARCHAR (50)  NULL,
    [EmailSentOn]            VARCHAR (50)  NULL,
    [RequestType]            INT           NULL,
    [ApprovalDate]           DATETIME      NULL,
    [SentDate]               DATETIME      NULL,
    [PIN]                    VARCHAR (100) NULL,
    [VoucherNo]              VARCHAR (250) NULL,
    [ExpiryDate]             DATETIME      NULL,
    [UserId]                 INT           NULL,
    [TC_EWalletsId]          INT           NULL,
    [PayUMoneyTransactionId] INT           NULL,
    [PayUMoneyStatus]        BIT           NULL,
    [PayUMoneyMessage]       VARCHAR (500) NULL,
    [RedeemedAmount]         INT           NULL
);

