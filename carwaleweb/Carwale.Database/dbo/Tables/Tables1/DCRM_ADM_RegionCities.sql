CREATE TABLE [dbo].[DCRM_ADM_RegionCities] (
    [RegionId] INT          NOT NULL,
    [CityId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_ADM_RegionCities] PRIMARY KEY CLUSTERED ([RegionId] ASC, [CityId] ASC)
);

