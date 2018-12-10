CREATE TABLE [CRM].[LSCityTier] (
    [CityId]    NUMERIC (18) NOT NULL,
    [TierId]    INT          NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_LSCityTier_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LSCityTier] PRIMARY KEY CLUSTERED ([CityId] ASC)
);

