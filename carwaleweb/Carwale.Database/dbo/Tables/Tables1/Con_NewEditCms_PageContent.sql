CREATE TABLE [dbo].[Con_NewEditCms_PageContent] (
    [Id]     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [PageId] NUMERIC (18)  NOT NULL,
    [Data]   VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Con_NewEditCms_PageContent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

