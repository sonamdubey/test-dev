CREATE TABLE [dbo].[DealInquiries] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [CustomerName]   VARCHAR (50)  NULL,
    [CustomerEmail]  VARCHAR (100) NULL,
    [CustomerMobile] VARCHAR (10)  NULL,
    [StockId]        INT           NULL,
    [EntryDateTime]  DATETIME      NULL,
    [PushStatus]     INT           NULL,
    [CityId]         INT           NULL,
    [MasterCityId]   INT           NULL,
    [Source]         VARCHAR (5)   NULL,
    [IsPaid]         BIT           NULL,
    [PlatformId]     INT           NULL,
    [Eagerness]      INT           NULL,
    CONSTRAINT [PK_DealInquiries] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DealInquiries_StockId]
    ON [dbo].[DealInquiries]([StockId] ASC);

