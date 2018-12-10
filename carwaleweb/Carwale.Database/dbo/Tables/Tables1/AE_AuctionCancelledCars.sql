CREATE TABLE [dbo].[AE_AuctionCancelledCars] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AuctionCarId] NUMERIC (18)  NOT NULL,
    [Reason]       VARCHAR (500) NULL,
    [UpdatedBy]    NUMERIC (18)  NOT NULL,
    [UpdatedOn]    DATETIME      CONSTRAINT [DF_AE_AuctionCancelledCars_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_AE_AuctionCancelledCars] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

