CREATE TABLE [dbo].[Con_EditCms_Tags] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Tag]             VARCHAR (100) NULL,
    [slug]            VARCHAR (150) NULL,
    [LastUpdatedTime] DATETIME      CONSTRAINT [DF_Con_EditCms_Tags_LastUpdatedTime] DEFAULT (getdate()) NULL,
    [LastUpdatedBy]   NUMERIC (18)  NULL,
    [BWMigratedId]    INT           NULL,
    CONSTRAINT [PK_Con_EditCms_Tags] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Con_EditCms_Tags_slug]
    ON [dbo].[Con_EditCms_Tags]([slug] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Tags_tag]
    ON [dbo].[Con_EditCms_Tags]([Tag] ASC);

