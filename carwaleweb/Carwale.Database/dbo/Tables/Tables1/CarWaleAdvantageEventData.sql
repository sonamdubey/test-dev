CREATE TABLE [dbo].[CarWaleAdvantageEventData] (
    [Id]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [StockId]    BIGINT   NULL,
    [CityId]     INT      NULL,
    [CategoryId] TINYINT  NULL,
    [PlatformId] INT      NULL,
    [Counter]    BIGINT   NULL,
    [EventDate]  DATETIME NULL
);

