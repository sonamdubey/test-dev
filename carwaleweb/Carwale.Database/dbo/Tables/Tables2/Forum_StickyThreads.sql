CREATE TABLE [dbo].[Forum_StickyThreads] (
    [ID]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ThreadId]    NUMERIC (18) NOT NULL,
    [CatId]       SMALLINT     NOT NULL,
    [CreatedBy]   NUMERIC (18) NOT NULL,
    [CreatedDate] DATETIME     NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_Forum_StickyThreads_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Forum_StickyThreads] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

