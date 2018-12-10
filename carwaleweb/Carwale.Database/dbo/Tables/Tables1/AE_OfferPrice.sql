CREATE TABLE [dbo].[AE_OfferPrice] (
    [AuctionCarId] NUMERIC (18) NOT NULL,
    [OfferPrice]   NUMERIC (18) NOT NULL,
    [IpAddress]    VARCHAR (50) NOT NULL,
    [EntryDate]    DATETIME     NOT NULL,
    CONSTRAINT [PK_AE_OfferPrice] PRIMARY KEY CLUSTERED ([AuctionCarId] ASC) WITH (FILLFACTOR = 90)
);

