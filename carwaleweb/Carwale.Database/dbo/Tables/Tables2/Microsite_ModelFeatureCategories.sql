CREATE TABLE [dbo].[Microsite_ModelFeatureCategories] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName] VARCHAR (50) NULL,
    [IsActive]     BIT          CONSTRAINT [DF_Microsite_ModelFeatureCategories_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Microsite_ModelFeatureCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

