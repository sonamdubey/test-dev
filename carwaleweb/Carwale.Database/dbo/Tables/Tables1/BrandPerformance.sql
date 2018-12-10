CREATE TABLE [dbo].[BrandPerformance] (
    [Id]                   NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarModelId]           NUMERIC (18)    NULL,
    [ResearchPV]           NUMERIC (18)    NULL,
    [UsedListingLastYear]  NUMERIC (18)    NULL,
    [UsedListingLastMonth] NUMERIC (18)    NULL,
    [PQLastYear]           NUMERIC (18)    NULL,
    [PQLastMonth]          NUMERIC (18)    NULL,
    [ForumTillNow]         NUMERIC (18)    NULL,
    [Rating]               NUMERIC (18, 1) NULL,
    [Reviews]              NUMERIC (18)    NULL,
    [EntryDate]            DATETIME        NULL,
    CONSTRAINT [PK_BrandPerformance] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

