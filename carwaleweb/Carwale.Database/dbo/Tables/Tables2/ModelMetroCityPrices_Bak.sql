CREATE TABLE [dbo].[ModelMetroCityPrices_Bak] (
    [ModelMetroCityPricesId] INT      IDENTITY (1, 1) NOT NULL,
    [CarModelId]             INT      NOT NULL,
    [CityId]                 INT      NOT NULL,
    [MinPrice]               INT      NULL,
    [MaxPrice]               INT      NULL,
    [CreatedOn]              DATETIME NULL
);

