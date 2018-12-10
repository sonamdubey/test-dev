CREATE TABLE [dbo].[TC_PQRequestComponentLog] (
    [TC_PriceQuoteRequestsId]    INT      NOT NULL,
    [TC_PQComponentId]           INT      NOT NULL,
    [Amount]                     INT      NOT NULL,
    [EntryDate]                  DATETIME NULL,
    [TC_PQRequestComponentLogId] INT      IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_TC_PQRequestComponentLog] PRIMARY KEY CLUSTERED ([TC_PQRequestComponentLogId] ASC)
);

