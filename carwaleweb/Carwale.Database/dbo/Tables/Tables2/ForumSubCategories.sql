CREATE TABLE [dbo].[ForumSubCategories] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ForumCategoryId] NUMERIC (18)  NOT NULL,
    [Name]            VARCHAR (100) NOT NULL,
    [Description]     VARCHAR (500) NULL,
    [IsActive]        BIT           NOT NULL,
    [LastPostId]      NUMERIC (18)  NULL,
    [Threads]         NUMERIC (18)  NULL,
    [Posts]           NUMERIC (18)  NULL,
    [URL]             VARCHAR (200) NULL,
    CONSTRAINT [PK_ForumSubCategories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ForumSubCategories_ForumCategories] FOREIGN KEY ([ForumCategoryId]) REFERENCES [dbo].[ForumCategories] ([ID])
);

