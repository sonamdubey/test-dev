CREATE TABLE [dbo].[TC_Deals_StockPriceBreakup] (
    [TC_Deals_PriceBreakupId] INT           IDENTITY (1, 1) NOT NULL,
    [ExShowroom]              INT           NULL,
    [RTO]                     INT           NULL,
    [Insurance]               INT           NULL,
    [Accesories]              INT           NULL,
    [Customer_Care]           INT           NULL,
    [Incidental]              INT           NULL,
    [Handling_Logistics]      INT           NULL,
    [TCS]                     INT           NULL,
    [LBT]                     INT           NULL,
    [Depot]                   INT           NULL,
    [Other]                   INT           NULL,
    [Additional_Comments]     VARCHAR (300) NULL,
    [InsertedOn]              DATETIME      NOT NULL,
    [StockId]                 INT           NOT NULL,
    [CityId]                  INT           NOT NULL,
    [Facilitation]            INT           NULL,
    [Delivery]                INT           NULL,
    [Service]                 INT           NULL,
    [Registration]            INT           NULL,
    CONSTRAINT [PK_TC_Deals_PriceBreakupId] PRIMARY KEY CLUSTERED ([TC_Deals_PriceBreakupId] ASC)
);

