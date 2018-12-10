CREATE TABLE [dbo].[TC_MainCampaign] (
    [TC_MainCampaignId] INT           IDENTITY (1, 1) NOT NULL,
    [EntryDate]         DATETIME      NULL,
    [IsActive]          BIT           NULL,
    [MakeId]            INT           NULL,
    [MainCampaignName]  VARCHAR (150) NULL,
    CONSTRAINT [PK_TC_MainCampaign] PRIMARY KEY CLUSTERED ([TC_MainCampaignId] ASC)
);

