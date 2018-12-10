CREATE TABLE [dbo].[CommunityNews] (
    [ID]          NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]       VARCHAR (200)  NULL,
    [Description] VARCHAR (4000) NULL,
    [NewsDate]    DATETIME       NULL,
    CONSTRAINT [PK_CommunityNews] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

