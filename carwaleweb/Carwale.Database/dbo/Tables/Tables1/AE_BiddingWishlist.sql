CREATE TABLE [dbo].[AE_BiddingWishlist] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BidderId]     NUMERIC (18)   NOT NULL,
    [AuctionCarId] NUMERIC (18)   NOT NULL,
    [IsBidded]     BIT            CONSTRAINT [DF_AE_BiddingWishlist_IsBidded] DEFAULT ((0)) NOT NULL,
    [EntryDate]    DATETIME       NOT NULL,
    [AuctionId]    NUMERIC (9, 2) NULL,
    CONSTRAINT [PK_AE_BiddingWishlist] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

