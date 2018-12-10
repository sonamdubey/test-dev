CREATE TABLE [dbo].[ForumSearches] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SearchTerm]     VARCHAR (500) NOT NULL,
    [SearchDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_ForumSearches_0616] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ForumSearches__SearchDateTime_0616]
    ON [dbo].[ForumSearches]([SearchDateTime] ASC)
    INCLUDE([Id]);

