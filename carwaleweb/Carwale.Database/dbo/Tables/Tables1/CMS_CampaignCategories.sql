CREATE TABLE [dbo].[CMS_CampaignCategories] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_CMS_CampaignCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

