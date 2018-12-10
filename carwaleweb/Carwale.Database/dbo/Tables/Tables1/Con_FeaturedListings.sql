CREATE TABLE [dbo].[Con_FeaturedListings] (
    [ID]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]         NUMERIC (18)   NOT NULL,
    [IsModel]       BIT            NOT NULL,
    [Description]   VARCHAR (2000) NOT NULL,
    [EntryDateTime] DATETIME       NOT NULL,
    [IsVisible]     BIT            CONSTRAINT [Con_FeaturedListings_IsVisible] DEFAULT ((1)) NOT NULL,
    [IsActive]      BIT            NOT NULL,
    [ShowResearch]  BIT            NULL,
    [ShowPrice]     BIT            NULL,
    [ShowRoadTest]  BIT            NULL,
    [Link]          VARCHAR (200)  NULL,
    [HostURL]       VARCHAR (100)  DEFAULT ('img.carwale.com') NULL,
    [IsReplicated]  BIT            CONSTRAINT [DF__Con_Featu__IsRep__5C9241C9] DEFAULT ((0)) NULL,
    [DirectoryPath] VARCHAR (50)   NULL,
    [AdScript]      VARCHAR (500)  NULL,
    CONSTRAINT [PK_Con_FeaturedListings] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

