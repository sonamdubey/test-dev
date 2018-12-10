CREATE TABLE [dbo].[AbSure_CarsWithoutRCImage] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId] BIGINT   NULL,
    [DealerId]            INT      NULL,
    [StockId]             BIGINT   NULL,
    [EntryDate]           DATETIME CONSTRAINT [DF_AbSure_CarsWithoutRCImage_EntryDate] DEFAULT (getdate()) NULL
);

