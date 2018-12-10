CREATE TABLE [dbo].[FeaturedListings] (
    [ID]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]         NUMERIC (18)   NOT NULL,
    [IsModel]       BIT            NOT NULL,
    [Description]   VARCHAR (2000) NOT NULL,
    [EntryDateTime] DATETIME       NOT NULL,
    [IsVisible]     BIT            CONSTRAINT [DF_FeaturedListings_IsVisible] DEFAULT ((1)) NOT NULL,
    [IsActive]      BIT            CONSTRAINT [DF_FeaturedListings_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_FeaturedListings] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

