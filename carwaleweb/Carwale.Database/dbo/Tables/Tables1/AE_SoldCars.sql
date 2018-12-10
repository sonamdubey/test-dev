CREATE TABLE [dbo].[AE_SoldCars] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AuctionCarId] NUMERIC (18) NOT NULL,
    [BidderId]     NUMERIC (18) NOT NULL,
    [BasePrice]    NUMERIC (18) NULL,
    [SoldPrice]    NUMERIC (18) NOT NULL,
    [SoldDate]     DATETIME     NOT NULL,
    [SoldBy]       NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_AE_SoldCars] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

