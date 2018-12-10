CREATE TABLE [dbo].[BW_NewBikeDealerShowroomPricesBkp191114] (
    [Id]            BIGINT   IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT      NULL,
    [BikeVersionId] INT      NULL,
    [CityId]        INT      NULL,
    [ItemId]        SMALLINT NULL,
    [ItemValue]     INT      NULL,
    [EntryDate]     DATETIME NULL,
    [EnteredBy]     INT      NULL
);

