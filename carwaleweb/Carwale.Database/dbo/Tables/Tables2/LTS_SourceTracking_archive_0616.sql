CREATE TABLE [dbo].[LTS_SourceTracking_archive_0616] (
    [ID]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CampaignId]    NUMERIC (18)  NULL,
    [CampaignCode]  VARCHAR (100) NULL,
    [EntryDateTime] DATETIME      NULL,
    [IPAddress]     VARCHAR (50)  NULL,
    [LandingURL]    VARCHAR (100) NULL,
    [PreviousSTId]  NUMERIC (18)  NULL,
    [LeadId]        NUMERIC (18)  CONSTRAINT [DF_LTS_SourceTracking_LeadId22] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_Seo_Tracker21] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_LTS_SourceTracking_CampaignCode2]
    ON [dbo].[LTS_SourceTracking_archive_0616]([CampaignCode] ASC);

