CREATE TABLE [dbo].[NCD_Campaigns] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]       NUMERIC (18)  NOT NULL,
    [CampaignName]   VARCHAR (250) NOT NULL,
    [TotalLeadCount] BIGINT        NOT NULL,
    [CreatedBy]      NUMERIC (18)  NOT NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_NCD_Campaign_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      NUMERIC (18)  NULL,
    [UpdatedOn]      DATETIME      NULL,
    [IsActive]       BIT           NOT NULL,
    [IsDailyBased]   BIT           NOT NULL,
    [DailyCount]     BIGINT        NULL,
    CONSTRAINT [PK_NCD_Campaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

