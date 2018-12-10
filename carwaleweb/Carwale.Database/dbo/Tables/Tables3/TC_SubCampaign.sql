CREATE TABLE [dbo].[TC_SubCampaign] (
    [TC_SubCampaignId]     INT           IDENTITY (1, 1) NOT NULL,
    [TC_SubMainCampaignId] INT           NULL,
    [EntryDate]            DATETIME      NULL,
    [IsActive]             BIT           NULL,
    [SubCampaignName]      VARCHAR (200) NULL,
    CONSTRAINT [PK_TC_SubCampaign] PRIMARY KEY CLUSTERED ([TC_SubCampaignId] ASC)
);

