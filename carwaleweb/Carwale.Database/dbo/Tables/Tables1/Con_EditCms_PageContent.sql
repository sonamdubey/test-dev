CREATE TABLE [dbo].[Con_EditCms_PageContent] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [PageId]       NUMERIC (18)  NOT NULL,
    [Data]         VARCHAR (MAX) NOT NULL,
    [BWMigratedId] INT           NULL,
    [BWOldPageId]  INT           NULL,
    CONSTRAINT [PK_Con_EditCms_PageContent] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_PageContent__PageId]
    ON [dbo].[Con_EditCms_PageContent]([PageId] ASC)
    INCLUDE([Data]);

