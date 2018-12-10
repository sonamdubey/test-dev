CREATE TABLE [dbo].[IPtoLocation_CityMapping] (
    [ID]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [IPtoLocationCityName] VARCHAR (128) NULL,
    [CWCityName]           VARCHAR (50)  NULL,
    [CityId]               NUMERIC (18)  NULL
);


GO
CREATE CLUSTERED INDEX [IX_IPtoLocation_CityMapping_CityId]
    ON [dbo].[IPtoLocation_CityMapping]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_IPtoLocationCityName]
    ON [dbo].[IPtoLocation_CityMapping]([IPtoLocationCityName] ASC);

