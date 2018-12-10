CREATE TABLE [dbo].[CampaignCategory] (
    [Id]       SMALLINT     IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [IsActive] BIT          NULL,
    CONSTRAINT [PK_CampaignCategoryId] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

