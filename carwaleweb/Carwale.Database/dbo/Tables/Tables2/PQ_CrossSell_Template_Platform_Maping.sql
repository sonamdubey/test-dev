CREATE TABLE [dbo].[PQ_CrossSell_Template_Platform_Maping] (
    [ID]                  INT     IDENTITY (1, 1) NOT NULL,
    [CrossSellCampaignId] INT     NULL,
    [AssignedTemplateId]  INT     NULL,
    [PlatformId]          TINYINT NULL,
    CONSTRAINT [PQ_CTPMaping] PRIMARY KEY CLUSTERED ([ID] ASC)
);

