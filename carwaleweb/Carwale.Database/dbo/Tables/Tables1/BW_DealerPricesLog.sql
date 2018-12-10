CREATE TABLE [dbo].[BW_DealerPricesLog] (
    [Id]            BIGINT   IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT      NULL,
    [BikeVersionId] INT      NULL,
    [CityId]        INT      NULL,
    [ItemId]        SMALLINT NULL,
    [ItemValue]     INT      NULL,
    [LastUpdatedOn] DATETIME NULL,
    [UpdatedBy]     INT      NULL
);

