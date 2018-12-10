CREATE TABLE [dbo].[CRM_NCDCampaign] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]       NUMERIC (18)  NOT NULL,
    [CampaignType]   TINYINT       NULL,
    [StartDate]      DATETIME      NOT NULL,
    [EndDate]        DATETIME      NULL,
    [LeadCount]      BIGINT        NULL,
    [CreatedBy]      NUMERIC (18)  NOT NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_CRM_NCDCampaign_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      NUMERIC (18)  NULL,
    [UpdatedOn]      DATETIME      CONSTRAINT [DF_CRM_NCDCampaign_UpdatedOn] DEFAULT (getdate()) NULL,
    [CampaignName]   VARCHAR (250) NOT NULL,
    [IsActive]       BIT           NOT NULL,
    [IsAreaBased]    BIT           NOT NULL,
    [IsDailyBased]   BIT           NULL,
    [DailyCount]     BIGINT        NULL,
    [TotalLeadCount] BIGINT        NULL,
    CONSTRAINT [PK_CRM_NCDCampaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

