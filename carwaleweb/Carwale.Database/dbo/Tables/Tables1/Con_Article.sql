CREATE TABLE [dbo].[Con_Article] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CatId]        NUMERIC (18)  NOT NULL,
    [Title]        VARCHAR (250) NOT NULL,
    [DisplayDate]  DATETIME      NOT NULL,
    [AuthorName]   VARCHAR (100) NOT NULL,
    [Tags]         VARCHAR (500) NULL,
    [PageKeywords] VARCHAR (500) NULL,
    [PageDesc]     VARCHAR (500) NULL,
    [Synopsis]     VARCHAR (500) NULL,
    [EntryDate]    DATETIME      NOT NULL,
    [IsPublished]  BIT           CONSTRAINT [DF_Con_Article_IsPublished] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Con_Article] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

