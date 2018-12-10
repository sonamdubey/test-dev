CREATE TABLE [dbo].[HighlightedCarsBidding] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]    NUMERIC (18) NOT NULL,
    [CarId]       NUMERIC (18) NOT NULL,
    [Bid]         NUMERIC (18) NOT NULL,
    [BidDateTime] DATETIME     NOT NULL,
    [Used]        BIT          CONSTRAINT [DF_HighlightedCarsBidding_Used] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_HighlightedCarsBidding] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

