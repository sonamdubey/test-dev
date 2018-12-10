CREATE TABLE [dbo].[TC_StockChangesLog] (
    [TC_StockId]             BIGINT        NOT NULL,
    [BranchId]               BIGINT        NOT NULL,
    [Kms]                    INT           NULL,
    [MakeYear]               DATETIME      NULL,
    [ExpectedPrice]          INT           NULL,
    [SpecialNote]            VARCHAR (500) NULL,
    [EntryDate]              DATETIME      CONSTRAINT [EntryDate] DEFAULT (getdate()) NOT NULL,
    [SpecialNoteCharCount]   INT           NULL,
    [TC_ActionApplicationId] INT           NULL,
    [PurchaseCost]           INT           NULL,
    [RefurbishmentCost]      INT           NULL
);

