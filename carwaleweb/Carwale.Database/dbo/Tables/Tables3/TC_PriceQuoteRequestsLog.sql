CREATE TABLE [dbo].[TC_PriceQuoteRequestsLog] (
    [TC_PriceQuoteRequestsLogId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_PriceQuoteRequestsId]    INT      NOT NULL,
    [TC_InquiriesId]             BIGINT   NULL,
    [CityId]                     INT      NOT NULL,
    [VersionId]                  INT      NULL,
    [ExShowRoomPrice]            INT      NULL,
    [RTO]                        INT      NULL,
    [Insurance]                  INT      NULL,
    [OnRoadPrice]                INT      NULL,
    [UserId]                     INT      NULL,
    [PQDate]                     DATETIME NULL,
    CONSTRAINT [PK_TC_PriceQuoteRequestsLog] PRIMARY KEY CLUSTERED ([TC_PriceQuoteRequestsLogId] ASC)
);

