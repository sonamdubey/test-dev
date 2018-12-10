CREATE TABLE [dbo].[CRM_NCDAreaWiseCampaign] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]   INT      NOT NULL,
    [CampaignId] INT      NOT NULL,
    [AreaId]     INT      NOT NULL,
    [CreatedOn]  DATETIME CONSTRAINT [DF_CRM_NCDAreaWiseCampaign_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]  INT      NOT NULL,
    [UpdatedOn]  DATETIME NULL,
    [UpdatedBy]  INT      NULL,
    [IsDeleted]  BIT      NOT NULL
);

