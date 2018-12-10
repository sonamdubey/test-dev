CREATE TABLE [dbo].[PQ_CrossSellCampaign] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CampaignName] VARCHAR (100) NULL,
    [StartDate]    DATETIME      NULL,
    [EndDate]      DATETIME      NULL,
    [IsActive]     BIT           CONSTRAINT [DF__PQ_CrossS__IsAct__4B26F34E] DEFAULT ((1)) NULL,
    [UpdatedOn]    DATETIME      NULL,
    [UpdatedBy]    INT           NULL,
    [CampaignId]   INT           NULL,
    [AddedOn]      DATETIME      NULL,
    CONSTRAINT [PK_PQ_CrossSellCampaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

