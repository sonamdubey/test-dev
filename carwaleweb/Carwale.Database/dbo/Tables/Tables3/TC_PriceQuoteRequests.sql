CREATE TABLE [dbo].[TC_PriceQuoteRequests] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId]  BIGINT   NULL,
    [CityId]          INT      NOT NULL,
    [VersionId]       INT      NULL,
    [ExShowRoomPrice] INT      NULL,
    [RTO]             INT      NULL,
    [Insurance]       INT      NULL,
    [OnRoadPrice]     INT      NULL,
    [UserId]          INT      NULL,
    [PQDate]          DATETIME NULL,
    [ModifiedDate]    DATETIME NULL,
    [ModifiedBy]      INT      NULL,
    CONSTRAINT [PK_TC_PriseQuoteRequests] PRIMARY KEY CLUSTERED ([Id] ASC)
);

