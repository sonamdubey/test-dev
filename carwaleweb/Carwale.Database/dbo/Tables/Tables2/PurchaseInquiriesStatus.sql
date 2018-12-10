CREATE TABLE [dbo].[PurchaseInquiriesStatus] (
    [ID]   NUMERIC (18) NOT NULL,
    [Name] CHAR (10)    NULL,
    CONSTRAINT [PK_PurchaseInqStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

