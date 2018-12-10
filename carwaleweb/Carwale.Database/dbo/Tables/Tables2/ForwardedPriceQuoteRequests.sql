CREATE TABLE [dbo].[ForwardedPriceQuoteRequests] (
    [FPQ_Id]                    NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NewCarPriceQuoteRequestId] NUMERIC (18) NOT NULL,
    [DealerId]                  NUMERIC (18) NOT NULL,
    [ForwardDateTime]           DATETIME     NOT NULL,
    CONSTRAINT [PK_ForwardedPriceQuoteRequests] PRIMARY KEY CLUSTERED ([FPQ_Id] ASC) WITH (FILLFACTOR = 90)
);

