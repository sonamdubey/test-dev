CREATE TABLE [dbo].[FOB_DealerCities] (
    [DealerId] NUMERIC (18) NOT NULL,
    [CityId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_FOB_DealerCities] PRIMARY KEY CLUSTERED ([DealerId] ASC, [CityId] ASC) WITH (FILLFACTOR = 90)
);

