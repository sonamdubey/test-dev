CREATE TABLE [dbo].[SocialPluginsCount] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [TypeId]               INT            NULL,
    [Url]                  NVARCHAR (MAX) NULL,
    [FacebookLikecount]    INT            NULL,
    [FacebookCommentCount] INT            NULL,
    [TypeCategoryId]       INT            NULL,
    CONSTRAINT [PK_NewsSocialCount] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SocialPluginsCount]
    ON [dbo].[SocialPluginsCount]([TypeId] ASC, [TypeCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SocialPluginsCount_TypeId_1]
    ON [dbo].[SocialPluginsCount]([TypeId] ASC)
    INCLUDE([FacebookCommentCount]);

