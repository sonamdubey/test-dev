CREATE TABLE [dbo].[Microsite_DealerContentSubCategories] (
    [SubCatagoryId]    INT           NULL,
    [CategoryId]       INT           NULL,
    [SubCatagoryName]  VARCHAR (100) NULL,
    [IsActive]         BIT           NULL,
    [UrlValue]         VARCHAR (500) NULL,
    [DealerId]         BIGINT        NULL,
    [SubCategoryOrder] INT           NULL,
    [NavigationId]     INT           NULL,
    CONSTRAINT [FK_DealerContentSubCategories_NaviagtionPagesMap] FOREIGN KEY ([NavigationId]) REFERENCES [dbo].[DealerWebsite_NaviagtionPagesMap] ([ID])
);

