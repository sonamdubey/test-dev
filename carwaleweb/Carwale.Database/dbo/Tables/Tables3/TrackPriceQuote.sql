CREATE TABLE [dbo].[TrackPriceQuote] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Steps]         INT          NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_TrackPriceQuote] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

