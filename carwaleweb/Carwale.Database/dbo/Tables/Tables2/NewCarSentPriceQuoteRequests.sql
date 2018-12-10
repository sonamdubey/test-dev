CREATE TABLE [dbo].[NewCarSentPriceQuoteRequests] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuoteRequestId] NUMERIC (18) NOT NULL,
    [DealerId]       NUMERIC (18) NOT NULL,
    [IsViewed]       BIT          CONSTRAINT [DF_NewCarSentPriceQuoteRequests_IsViewed] DEFAULT (0) NOT NULL,
    [IsReplied]      BIT          CONSTRAINT [DF_NewCarSentPriceQuoteRequests_IsReplied] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_NewCarQuoteRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewCarQuoteRequests_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID])
);

