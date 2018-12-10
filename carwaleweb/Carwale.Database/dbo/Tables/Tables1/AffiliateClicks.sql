CREATE TABLE [dbo].[AffiliateClicks] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SiteCode]    VARCHAR (50) NOT NULL,
    [ClickDate]   DATETIME     NOT NULL,
    [TotalClicks] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_AffiliateClicks] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

