CREATE TABLE [dbo].[TC_Deals_Offers] (
    [StockId]            INT           NOT NULL,
    [CategoryId]         INT           NOT NULL,
    [OfferWorth]         INT           NULL,
    [AdditionalComments] VARCHAR (500) NULL,
    CONSTRAINT [pk_OfferId] PRIMARY KEY CLUSTERED ([StockId] ASC, [CategoryId] ASC)
);

