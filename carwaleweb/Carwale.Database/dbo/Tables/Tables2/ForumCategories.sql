CREATE TABLE [dbo].[ForumCategories] (
    [ID]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Description] VARCHAR (500) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_ForumCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ForumCategories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

