CREATE TABLE [dbo].[ForumSearchResults] (
    [SearchId]      CHAR (10) NOT NULL,
    [ForumThreadId] CHAR (10) NOT NULL,
    [IsTitleMatch]  BIT       NOT NULL,
    CONSTRAINT [PK_ForumSearchResults] PRIMARY KEY CLUSTERED ([SearchId] ASC, [ForumThreadId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ForumSearchResults__IsTitleMatch]
    ON [dbo].[ForumSearchResults]([IsTitleMatch] ASC)
    INCLUDE([SearchId], [ForumThreadId]);

