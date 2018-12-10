CREATE TABLE [dbo].[MM_AvailableNumbers] (
    [ID]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MaskingNumber]   VARCHAR (15) NOT NULL,
    [CityId]          NUMERIC (18) NOT NULL,
    [ServiceProvider] TINYINT      DEFAULT ((1)) NOT NULL,
    [StateId]         INT          NULL
);

