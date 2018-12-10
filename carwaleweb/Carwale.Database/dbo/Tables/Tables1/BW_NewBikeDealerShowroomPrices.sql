CREATE TABLE [dbo].[BW_NewBikeDealerShowroomPrices] (
    [Id]            BIGINT   IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT      NULL,
    [BikeVersionId] INT      NULL,
    [CityId]        INT      NULL,
    [ItemId]        SMALLINT NULL,
    [ItemValue]     INT      NULL,
    [EntryDate]     DATETIME NULL,
    [EnteredBy]     INT      NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_BW_NewBikeDealerShowroomPrices_ItemId]
    ON [dbo].[BW_NewBikeDealerShowroomPrices]([ItemId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BW_NewBikeDealerShowroomPrices_VersionId]
    ON [dbo].[BW_NewBikeDealerShowroomPrices]([BikeVersionId] ASC)
    INCLUDE([DealerId], [CityId]);


GO
CREATE NONCLUSTERED INDEX [IX_BW_NewBikeDealerShowroomPrices_Dealerid]
    ON [dbo].[BW_NewBikeDealerShowroomPrices]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BW_NewBikeDealerShowroomPrices_CityId]
    ON [dbo].[BW_NewBikeDealerShowroomPrices]([CityId] ASC);

