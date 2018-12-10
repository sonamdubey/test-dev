CREATE TABLE [dbo].[Con_NewEditCms_Images] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]         NUMERIC (18)  NULL,
    [ImageCategoryId] NUMERIC (18)  NULL,
    [Caption]         VARCHAR (250) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Con_NewEditCms_Images_IsActive] DEFAULT ((1)) NULL,
    [LastUpdatedTime] DATETIME      NULL,
    [LastUpdatedBy]   NUMERIC (18)  NULL,
    [Sequence]        INT           CONSTRAINT [DF_Con_NewEditCms_Images_Sequence] DEFAULT ((0)) NULL,
    [RTImageId]       INT           NULL,
    CONSTRAINT [PK_Con_NewEditCms_Images] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

