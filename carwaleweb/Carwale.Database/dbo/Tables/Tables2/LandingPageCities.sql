CREATE TABLE [dbo].[LandingPageCities] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [CampaignId] INT NOT NULL,
    [StateId]    INT NOT NULL,
    [CityId]     INT NOT NULL,
    CONSTRAINT [PK_LandingPageCities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

