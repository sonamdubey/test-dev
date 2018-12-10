CREATE TABLE [dbo].[TC_SubMainCampaign] (
    [TC_SubMainCampaignId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_MainCampaignId]    INT           NULL,
    [EntryDate]            DATETIME      NULL,
    [IsActive]             BIT           NULL,
    [SubMainCampaignName]  VARCHAR (150) NULL,
    CONSTRAINT [PK_TC_SubMainCampaign] PRIMARY KEY CLUSTERED ([TC_SubMainCampaignId] ASC)
);

