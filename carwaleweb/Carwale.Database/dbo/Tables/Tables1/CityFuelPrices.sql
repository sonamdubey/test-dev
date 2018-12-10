CREATE TABLE [dbo].[CityFuelPrices] (
    [CityId]    SMALLINT       NOT NULL,
    [FuelPrice] DECIMAL (5, 2) NOT NULL,
    [FuelType]  NCHAR (10)     NOT NULL,
    [IsActive]  BIT            CONSTRAINT [DF_CityFuelPrices_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CityFuelPrices] PRIMARY KEY CLUSTERED ([CityId] ASC, [FuelType] ASC) WITH (FILLFACTOR = 90)
);

