CREATE TABLE [dbo].[AE_AuctionCars] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AuctionId]          NUMERIC (18) NOT NULL,
    [CarId]              NUMERIC (18) NOT NULL,
    [ReservePrice]       NUMERIC (18) NULL,
    [BasePrice]          NUMERIC (18) NOT NULL,
    [TopBidder]          NUMERIC (18) NULL,
    [HighestBid]         NUMERIC (18) CONSTRAINT [DF_AE_AuctionCars_HighestBid] DEFAULT ((0)) NULL,
    [TotalBids]          NUMERIC (18) CONSTRAINT [DF_AE_AuctionCars_TotalBids] DEFAULT ((0)) NULL,
    [LastBidDateTime]    DATETIME     NULL,
    [StartDateTime]      DATETIME     NULL,
    [InitialClosingTime] DATETIME     NULL,
    [FinalClosingTime]   DATETIME     NULL,
    [StatusId]           SMALLINT     NULL,
    [CreatedOn]          DATETIME     NOT NULL,
    [UpdatedOn]          DATETIME     NULL,
    [UpdatedBy]          NUMERIC (18) NULL,
    CONSTRAINT [PK_AE_AuctionCars] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Available = 1,
Blocked = 2,
Sold = 3,
Forfeit = 4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AE_AuctionCars', @level2type = N'COLUMN', @level2name = N'StatusId';

