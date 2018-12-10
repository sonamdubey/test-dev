CREATE TABLE [dbo].[CarValuesCityDeviation] (
    [CityId]    NUMERIC (18) NOT NULL,
    [Deviation] FLOAT (53)   NOT NULL,
    [GuideId]   NUMERIC (18) CONSTRAINT [DF_CarValuesCityDeviationNew_GuideId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CarValuesCityDeviationNew] PRIMARY KEY CLUSTERED ([CityId] ASC, [GuideId] ASC) WITH (FILLFACTOR = 90)
);

