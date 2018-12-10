CREATE TABLE [dbo].[ListingAnalysis] (
    [CarModelId]        NUMERIC (18) NOT NULL,
    [Listings]          NUMERIC (18) CONSTRAINT [DF_ListingAnalysis_Listings] DEFAULT (0) NOT NULL,
    [ProfileViews]      NUMERIC (18) CONSTRAINT [DF_ListingAnalysis_ProfileViews] DEFAULT (0) NOT NULL,
    [PurchaseInquiries] NUMERIC (18) CONSTRAINT [DF_ListingAnalysis_PurchaseInquiries] DEFAULT (0) NOT NULL,
    [Valuations]        NUMERIC (18) CONSTRAINT [DF_ListingAnalysis_Valuations] DEFAULT (0) NOT NULL,
    [LastUpdated]       DATETIME     CONSTRAINT [DF_ListingAnalysis_LastUpdated] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_ListingAnalysis] PRIMARY KEY CLUSTERED ([CarModelId] ASC) WITH (FILLFACTOR = 90)
);

