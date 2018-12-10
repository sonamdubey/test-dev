CREATE TABLE [dbo].[CRM_NCDCampaignLog] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CampaignId]           NUMERIC (18)  NOT NULL,
    [CampaignType]         TINYINT       NOT NULL,
    [PreviousCampaignName] VARCHAR (250) NOT NULL,
    [NewCampaignName]      VARCHAR (250) NOT NULL,
    [PreviousLeadCount]    BIGINT        NULL,
    [NewLeadCount]         BIGINT        NULL,
    [PreviousStartDate]    DATETIME      NULL,
    [NewStartDate]         DATETIME      NULL,
    [PreviousEndDate]      DATETIME      NULL,
    [NewEndDate]           DATETIME      NULL,
    [UpdatedOn]            DATETIME      NULL,
    [UpdatedBy]            NUMERIC (18)  NULL,
    [IsActive]             BIT           NOT NULL,
    [IsAreaBased]          BIT           NOT NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_CRM_NCDCampaignLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            NUMERIC (18)  NOT NULL,
    [IsDailyBased]         BIT           NULL,
    [PreviousDailyCount]   BIGINT        NULL,
    CONSTRAINT [PK_CRM_NCDCampaignLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

