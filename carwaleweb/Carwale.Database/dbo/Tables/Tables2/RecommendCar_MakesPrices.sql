CREATE TABLE [dbo].[RecommendCar_MakesPrices] (
    [ID]              NUMERIC (18)  NOT NULL,
    [Name]            VARCHAR (150) NULL,
    [MinPriceNew]     NUMERIC (18)  NULL,
    [MaxPriceNew]     NUMERIC (18)  NULL,
    [MinPriceNewCar]  VARCHAR (150) NULL,
    [MinPriceUsed]    NUMERIC (18)  NULL,
    [MaxPriceUsed]    NUMERIC (18)  NULL,
    [MinPriceUsedCar] VARCHAR (150) NULL,
    CONSTRAINT [PK_RecommendCar_MakesPrices] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

