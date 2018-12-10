CREATE TABLE [dbo].[NCD_CampaignLog] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CampaignId]           NUMERIC (18)  NULL,
    [PreviousCampaignName] VARCHAR (250) NULL,
    [NewCampaignName]      VARCHAR (250) NULL,
    [PreviousLeadCount]    BIGINT        NULL,
    [NewLeadCount]         BIGINT        NULL,
    [UpdatedOn]            DATETIME      NULL,
    [UpdatedBy]            NUMERIC (18)  NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_NCD_CampaignLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            NUMERIC (18)  NOT NULL,
    [IsActive]             BIT           NOT NULL,
    [IsDailyBased]         BIT           NULL,
    [PreviousDailyCount]   BIGINT        NULL,
    CONSTRAINT [PK_NCD_CampaignLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

