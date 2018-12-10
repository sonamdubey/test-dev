CREATE TABLE [dbo].[TopCities] (
    [CityId]   NUMERIC (18) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_TopCities_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_TopCities] PRIMARY KEY CLUSTERED ([CityId] ASC) WITH (FILLFACTOR = 90)
);

