CREATE TABLE [dbo].[ForumSearches_archive_0616] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SearchTerm]     VARCHAR (500) NOT NULL,
    [SearchDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_ForumSearches] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ForumSearches__SearchDateTime]
    ON [dbo].[ForumSearches_archive_0616]([SearchDateTime] ASC)
    INCLUDE([Id]);

