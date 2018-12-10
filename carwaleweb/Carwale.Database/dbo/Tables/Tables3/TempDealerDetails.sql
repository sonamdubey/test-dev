CREATE TABLE [dbo].[TempDealerDetails] (
    [DealerId]         NUMERIC (18) NOT NULL,
    [StockCarsCount]   INT          NULL,
    [SellingInMonth]   INT          NULL,
    [UsingPc]          BIT          CONSTRAINT [DF_TempDealerDetails_UsingPc] DEFAULT ((0)) NOT NULL,
    [UsingSoftware]    BIT          CONSTRAINT [DF_TempDealerDetails_UsingSoftware] DEFAULT ((0)) NOT NULL,
    [UsingTradingCars] BIT          CONSTRAINT [DF_TempDealerDetails_UsingTradingCars] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TempDealerDetails] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

