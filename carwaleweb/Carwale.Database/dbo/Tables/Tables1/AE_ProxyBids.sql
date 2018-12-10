CREATE TABLE [dbo].[AE_ProxyBids] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AuctionCarId] NUMERIC (18) NOT NULL,
    [BidderId]     NUMERIC (18) NOT NULL,
    [BidAmount]    NUMERIC (18) NOT NULL,
    [BidDateTime]  DATETIME     NOT NULL,
    CONSTRAINT [PK_AE_ProxyBid] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

