CREATE TABLE [dbo].[LTS_Campaigns] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SourceId]     NUMERIC (18)  NULL,
    [CampaignCode] VARCHAR (100) NULL,
    [CampaignName] VARCHAR (100) NULL,
    [CampaignDesc] VARCHAR (500) NULL,
    [StartDate]    DATETIME      NULL,
    [IsActive]     BIT           CONSTRAINT [DF_LTS_Campaigns_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_LTS_Campaigns] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_LTS_campaigns__CampaignCode]
    ON [dbo].[LTS_Campaigns]([CampaignCode] ASC);

