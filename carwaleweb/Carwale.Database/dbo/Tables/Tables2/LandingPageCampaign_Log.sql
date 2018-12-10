CREATE TABLE [dbo].[LandingPageCampaign_Log] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [CampaignId] INT           NOT NULL,
    [Type]       VARCHAR (100) NOT NULL,
    [CreatedOn]  DATETIME      NOT NULL,
    [CreatedBy]  INT           NOT NULL,
    [UpdatedOn]  DATETIME      NOT NULL,
    [UpdatedBy]  INT           NOT NULL,
    [Changes]    VARCHAR (MAX) NOT NULL,
    [LogMessage] VARCHAR (500) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

