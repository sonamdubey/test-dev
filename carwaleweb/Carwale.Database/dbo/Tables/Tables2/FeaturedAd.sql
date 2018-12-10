CREATE TABLE [dbo].[FeaturedAd] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [Name]                 VARCHAR (100) NOT NULL,
    [StartDate]            DATE          NOT NULL,
    [EndDate]              DATE          NOT NULL,
    [IsActive]             BIT           NOT NULL,
    [UpdatedOn]            DATETIME      NOT NULL,
    [UpdatedBy]            INT           NOT NULL,
    [CampaignCategoryId]   INT           NULL,
    [ExternalLinkText]     VARCHAR (100) NULL,
    [LinkClickTracker]     VARCHAR (250) NULL,
    [CarNameClickTracker]  VARCHAR (250) NULL,
    [CarImageClickTracker] VARCHAR (250) NULL,
    CONSTRAINT [PK_FeaturedAd] PRIMARY KEY CLUSTERED ([Id] ASC)
);

