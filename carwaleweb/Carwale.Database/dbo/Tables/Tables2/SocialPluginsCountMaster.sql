CREATE TABLE [dbo].[SocialPluginsCountMaster] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [TypeCategoryId] INT           NULL,
    [Description]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_SocialPluginsCountMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

