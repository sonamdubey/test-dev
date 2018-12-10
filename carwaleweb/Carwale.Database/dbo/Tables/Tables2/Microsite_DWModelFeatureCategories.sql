CREATE TABLE [dbo].[Microsite_DWModelFeatureCategories] (
    [Id]                                 INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]                           INT      NULL,
    [DWModelId]                          INT      NULL,
    [Microsite_ModelFeatureCategoriesId] INT      NULL,
    [SortOrder]                          INT      NULL,
    [IsActive]                           BIT      CONSTRAINT [DF_Microsite_DWModelFeatureCategories_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]                          DATETIME CONSTRAINT [DF_Microsite_DWModelFeatureCategories_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]                       DATETIME NULL,
    CONSTRAINT [PK_Microsite_DWModelFeatureCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

