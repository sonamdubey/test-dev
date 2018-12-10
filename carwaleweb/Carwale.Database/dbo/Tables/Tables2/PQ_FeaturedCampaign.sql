CREATE TABLE [dbo].[PQ_FeaturedCampaign] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CampaignName] VARCHAR (100) NULL,
    [StartDate]    VARCHAR (50)  NULL,
    [EndDate]      VARCHAR (50)  NULL,
    [CampaignType] SMALLINT      NULL,
    [DealerId]     INT           NULL,
    [IsActive]     BIT           CONSTRAINT [DF_PQ_FeaturedCampaign_IsActive] DEFAULT ((1)) NULL,
    [UpdatedOn]    DATETIME      NULL,
    [UpdatedBy]    INT           NULL,
    CONSTRAINT [PK_PQ_FeaturedCampaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

