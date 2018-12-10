CREATE TABLE [dbo].[PurchaseInquiries] (
    [ID]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]        NUMERIC (18)   NOT NULL,
    [CarModelId]      NUMERIC (18)   NOT NULL,
    [CarVersionId]    NUMERIC (18)   NOT NULL,
    [EntryDate]       DATETIME       NOT NULL,
    [StatusId]        NUMERIC (18)   NOT NULL,
    [Comments]        VARCHAR (1000) NULL,
    [IsArchived]      BIT            CONSTRAINT [DF_PurchaseInquiries_IsArchived] DEFAULT (0) NOT NULL,
    [UpdateTimeStamp] VARCHAR (100)  NULL,
    CONSTRAINT [PK_PurchaseInq] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PurchaseInq_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_PurchaseInq_PurchaseInqStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[PurchaseInquiriesStatus] ([ID]) ON UPDATE CASCADE
);

