CREATE TABLE [dbo].[HighlightsOfTheDay] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerID]      NUMERIC (18) NOT NULL,
    [CarId]         NUMERIC (18) NOT NULL,
    [BidDateTime]   DATETIME     NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_HighlightsOfTheDay] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

