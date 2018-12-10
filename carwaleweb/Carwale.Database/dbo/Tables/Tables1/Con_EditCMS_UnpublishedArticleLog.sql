CREATE TABLE [dbo].[Con_EditCMS_UnpublishedArticleLog] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [BasicId]           INT           NULL,
    [ReasonToUnpublish] VARCHAR (250) NULL,
    [UnpublishBy]       INT           NULL,
    [UnpublishDate]     DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

