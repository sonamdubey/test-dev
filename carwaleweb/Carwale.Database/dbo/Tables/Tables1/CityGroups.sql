CREATE TABLE [dbo].[CityGroups] (
    [CityId]     NUMERIC (18) NOT NULL,
    [MainCityId] NUMERIC (18) NOT NULL,
    [IsActive]   BIT          NOT NULL,
    CONSTRAINT [PK_CityGroups] PRIMARY KEY CLUSTERED ([CityId] ASC, [MainCityId] ASC) WITH (FILLFACTOR = 90)
);

