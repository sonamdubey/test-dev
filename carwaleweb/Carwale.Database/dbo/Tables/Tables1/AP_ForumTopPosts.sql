CREATE TABLE [dbo].[AP_ForumTopPosts] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]    NUMERIC (18)  NULL,
    [CustomerName]  VARCHAR (150) NULL,
    [Posts]         NUMERIC (18)  NULL,
    [PostType]      SMALLINT      NULL,
    [LastUpdatedOn] DATETIME      NULL,
    CONSTRAINT [PK_AP_ForumTopPosts] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-For Top Contributers, 2-For CurrentTop Contributers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AP_ForumTopPosts', @level2type = N'COLUMN', @level2name = N'PostType';

