CREATE TABLE [dbo].[Forum_ArticleAssociation] (
    [ID]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ArticleType] SMALLINT     NULL,
    [ThreadId]    NUMERIC (18) NULL,
    [ArticleId]   NUMERIC (18) NULL,
    [CreateDate]  DATETIME     NULL,
    CONSTRAINT [PK_Forum_ArticleAssociation] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Forum_ArticleAssociation__ArticleId_ArticleType]
    ON [dbo].[Forum_ArticleAssociation]([ArticleId] ASC, [ArticleType] ASC);

