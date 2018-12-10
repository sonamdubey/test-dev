CREATE TABLE [dbo].[AW_ClientCampaigns] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ClientId]     NUMERIC (18)  NOT NULL,
    [CampaignName] VARCHAR (200) NOT NULL,
    [AWCampaignId] NUMERIC (18)  NOT NULL,
    [StartDate]    DATETIME      NOT NULL,
    [EndDate]      DATETIME      NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [CampaignType] SMALLINT      CONSTRAINT [DF_AW_ClientCampaigns_CampaignType] DEFAULT (1) NOT NULL,
    [BookedValue]  NUMERIC (18)  NULL,
    CONSTRAINT [PK_AW_ClientCampaigns] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

