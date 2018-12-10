CREATE TABLE [dbo].[LL_Cities] (
    [CityId]    NUMERIC (18)    NOT NULL,
    [CityName]  VARCHAR (100)   NULL,
    [Lattitude] DECIMAL (18, 4) NULL,
    [Longitude] DECIMAL (18, 4) NULL,
    CONSTRAINT [PK_LL_Cities] PRIMARY KEY CLUSTERED ([CityId] ASC) WITH (FILLFACTOR = 90)
);

