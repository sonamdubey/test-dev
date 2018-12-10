CREATE TABLE [dbo].[CarValuesCityDeviation_31Dec06] (
    [CityId]    NUMERIC (18) NOT NULL,
    [Deviation] INT          NOT NULL,
    [GuideId]   NUMERIC (18) CONSTRAINT [DF_CarValuesCityDeviation_GuideId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CarValuesCityDeviation] PRIMARY KEY CLUSTERED ([CityId] ASC, [GuideId] ASC) WITH (FILLFACTOR = 90)
);

