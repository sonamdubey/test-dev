CREATE TABLE [dbo].[SellInquiriesStatus] (
    [ID]   NUMERIC (18) NOT NULL,
    [Code] VARCHAR (4)  NULL,
    [Name] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_SellInqStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

