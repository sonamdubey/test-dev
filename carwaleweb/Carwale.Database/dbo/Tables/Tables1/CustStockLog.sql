CREATE TABLE [dbo].[CustStockLog] (
    [Id]                INT      IDENTITY (1, 1) NOT NULL,
    [CustSellInquiryId] INT      NOT NULL,
    [EntryTime]         DATETIME NOT NULL,
    [ActionType]        TINYINT  NOT NULL,
    [Action]            CHAR (6) NOT NULL,
    CONSTRAINT [PK_CustStockLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

