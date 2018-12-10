﻿CREATE TABLE [dbo].[NCS_FAZoneCities] (
    [FAZoneId] NUMERIC (18) NOT NULL,
    [CityId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_NCS_FAZoneCities] PRIMARY KEY CLUSTERED ([FAZoneId] ASC, [CityId] ASC) WITH (FILLFACTOR = 90)
);

