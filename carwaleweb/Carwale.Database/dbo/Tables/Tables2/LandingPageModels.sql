CREATE TABLE [dbo].[LandingPageModels] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [CampaignId] INT NOT NULL,
    [MakeId]     INT NOT NULL,
    [ModelId]    INT NOT NULL,
    CONSTRAINT [PK_LandingPageModels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

