CREATE TABLE [dbo].[News] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MakeId]         NUMERIC (18)  NOT NULL,
    [Title]          VARCHAR (200) NOT NULL,
    [Summary]        VARCHAR (500) NULL,
    [NewsDate]       DATETIME      NOT NULL,
    [Source]         VARCHAR (50)  NULL,
    [SourceLink]     VARCHAR (500) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_News_IsActive] DEFAULT (1) NOT NULL,
    [IsPressRelease] BIT           CONSTRAINT [DF_News_IsPressRelease] DEFAULT (0) NULL,
    [IsPublished]    BIT           CONSTRAINT [DF_News_IsPublished] DEFAULT (0) NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

