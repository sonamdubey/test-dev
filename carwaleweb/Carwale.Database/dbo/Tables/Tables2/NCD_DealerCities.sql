CREATE TABLE [dbo].[NCD_DealerCities] (
    [RegionID] INT NOT NULL,
    [CityID]   INT NOT NULL,
    CONSTRAINT [PK_NCD_DealerCities_1] PRIMARY KEY CLUSTERED ([RegionID] ASC, [CityID] ASC)
);

