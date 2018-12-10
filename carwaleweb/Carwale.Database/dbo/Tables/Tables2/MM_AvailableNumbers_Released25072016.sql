CREATE TABLE [dbo].[MM_AvailableNumbers_Released25072016] (
    [ID]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MaskingNumber]   VARCHAR (15) NOT NULL,
    [CityId]          NUMERIC (18) NOT NULL,
    [ServiceProvider] TINYINT      NOT NULL
);

