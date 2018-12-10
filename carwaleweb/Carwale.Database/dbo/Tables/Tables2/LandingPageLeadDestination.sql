CREATE TABLE [dbo].[LandingPageLeadDestination] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [CampaignId]   INT NOT NULL,
    [Type]         INT NOT NULL,
    [PQCampaignId] INT NULL,
    [DealerId]     INT NULL,
    CONSTRAINT [PK_LandingPageLeadDestination] PRIMARY KEY CLUSTERED ([Id] ASC)
);

