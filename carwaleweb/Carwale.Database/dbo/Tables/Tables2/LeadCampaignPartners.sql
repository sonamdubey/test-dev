CREATE TABLE [dbo].[LeadCampaignPartners] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CampaignName] VARCHAR (200) NOT NULL,
    [LeadCount]    INT           CONSTRAINT [DF_LeadCampaignPartners_LeadCount] DEFAULT (0) NOT NULL,
    [StartingFrom] DATETIME      NOT NULL,
    [EndingTo]     DATETIME      NOT NULL,
    [CampaignType] SMALLINT      NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_AdCampaignPartners_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_AdCampaignPartners] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

