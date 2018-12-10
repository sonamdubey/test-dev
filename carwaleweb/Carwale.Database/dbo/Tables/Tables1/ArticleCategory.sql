CREATE TABLE [dbo].[ArticleCategory] (
    [CategoryId]   NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryName] VARCHAR (35) NOT NULL,
    [IsActive]     BIT          NOT NULL,
    CONSTRAINT [PK_ArticleCategory] PRIMARY KEY CLUSTERED ([CategoryId] ASC) WITH (FILLFACTOR = 90)
);

