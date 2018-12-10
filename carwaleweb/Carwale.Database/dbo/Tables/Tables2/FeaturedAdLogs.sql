CREATE TABLE [dbo].[FeaturedAdLogs] (
    [FeaturedAdId]         INT           NULL,
    [FeaturedAdName]       VARCHAR (100) NULL,
    [StartDate]            DATETIME      NULL,
    [EndDate]              DATETIME      NULL,
    [IsActive]             BIT           NULL,
    [UpdatedOn]            DATETIME      NULL,
    [UpdatedBy]            INT           NULL,
    [Remarks]              VARCHAR (50)  NULL,
    [CampaignCategoryId]   INT           NULL,
    [ExternalLinkText]     VARCHAR (100) NULL,
    [LinkClickTracker]     VARCHAR (250) NULL,
    [CarNameClickTracker]  VARCHAR (250) NULL,
    [CarImageClickTracker] VARCHAR (250) NULL
);

