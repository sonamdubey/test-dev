CREATE TABLE [dbo].[PQ_DealerAd_Template_Platform_Maping] (
    [ID]                 INT     IDENTITY (1, 1) NOT NULL,
    [CampaignId]         INT     NULL,
    [AssignedTemplateId] INT     NULL,
    [PlatformId]         TINYINT NULL,
    [AssignedGroupId]    INT     NULL,
    CONSTRAINT [PK_PQ_DTPlatMap] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_DealerAd_Template_Platform_Maping_CampaignId]
    ON [dbo].[PQ_DealerAd_Template_Platform_Maping]([CampaignId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_pq_dealerad_template_platform_maping_AssignedTemplateId]
    ON [dbo].[PQ_DealerAd_Template_Platform_Maping]([AssignedTemplateId] ASC);

