CREATE TABLE [dbo].[CustomerTradeinInquiries] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerID]      NUMERIC (18) NOT NULL,
    [SellId]          NUMERIC (18) NOT NULL,
    [PurchaseId]      NUMERIC (18) NOT NULL,
    [RequestDateTime] DATETIME     NOT NULL,
    [IsApproved]      BIT          CONSTRAINT [DF_CustomerTradeinInquiries_IsApproved] DEFAULT (0) NOT NULL,
    [IsFake]          BIT          CONSTRAINT [DF_CustomerTradeinInquiries_IsFake] DEFAULT (0) NOT NULL,
    [StatusId]        SMALLINT     CONSTRAINT [DF_CustomerTradeinInquiries_StatusId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CustomerTradeinInquiries] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

