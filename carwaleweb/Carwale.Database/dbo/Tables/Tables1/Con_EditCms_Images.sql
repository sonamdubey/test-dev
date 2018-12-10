CREATE TABLE [dbo].[Con_EditCms_Images] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]              NUMERIC (18)  NULL,
    [ImageCategoryId]      NUMERIC (18)  NULL,
    [Caption]              VARCHAR (250) NULL,
    [IsActive]             BIT           CONSTRAINT [DF_Con_EditCms_Images_IsActive] DEFAULT ((1)) NULL,
    [LastUpdatedTime]      DATETIME      NULL,
    [LastUpdatedBy]        NUMERIC (18)  NULL,
    [Sequence]             INT           CONSTRAINT [DF_Con_EditCms_Images_Sequence] DEFAULT ((0)) NULL,
    [RTImageId]            INT           NULL,
    [IsUpdated]            BIT           CONSTRAINT [DF__Con_EditC__IsUpd__76F1324A] DEFAULT ((0)) NULL,
    [IsReplicated]         BIT           CONSTRAINT [DF__Con_EditC__IsRep__3F6BF336] DEFAULT ((0)) NULL,
    [HostURL]              VARCHAR (250) CONSTRAINT [DF__Con_EditC__HostU__61C10B3A] DEFAULT ('img.carwale.com') NULL,
    [HasCustomImg]         BIT           CONSTRAINT [DF_Con_EditCms_Images_HasCustomImg] DEFAULT ((0)) NULL,
    [MakeId]               INT           NULL,
    [ModelId]              INT           NULL,
    [IsMainImage]          BIT           CONSTRAINT [DF__Con_EditC__IsMai__7B21B5E1] DEFAULT ((0)) NULL,
    [ImageName]            VARCHAR (350) NULL,
    [ImagePathThumbnail]   VARCHAR (150) NULL,
    [ImagePathLarge]       VARCHAR (150) NULL,
    [ImagePathOriginal]    VARCHAR (150) NULL,
    [ImagePathCustom]      VARCHAR (150) NULL,
    [StatusId]             SMALLINT      CONSTRAINT [DF_Con_EditCms_Images_StatusId] DEFAULT ((1)) NULL,
    [ImagePathCustom88]    VARCHAR (150) NULL,
    [ImagePathCustom140]   VARCHAR (150) NULL,
    [ImagePathCustom200]   VARCHAR (150) NULL,
    [AltImageName]         VARCHAR (100) NULL,
    [Title]                VARCHAR (100) NULL,
    [Description]          VARCHAR (200) NULL,
    [BWMigratedId]         INT           NULL,
    [BWOldBasicId]         INT           NULL,
    [BWOldImageCategoryId] INT           NULL,
    [VideoPathThumbNail]   VARCHAR (150) NULL,
    [ImagePathSmall]       VARCHAR (150) NULL,
    [OriginalImgPath]      VARCHAR (150) NULL,
    CONSTRAINT [PK_Con_EditCms_Images] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Images]
    ON [dbo].[Con_EditCms_Images]([IsActive] ASC, [ModelId] ASC)
    INCLUDE([BasicId], [ImageCategoryId], [MakeId]);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Images_ModelId]
    ON [dbo].[Con_EditCms_Images]([ModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Images_StatusId]
    ON [dbo].[Con_EditCms_Images]([StatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Images_IsActive]
    ON [dbo].[Con_EditCms_Images]([IsActive] ASC, [IsMainImage] ASC)
    INCLUDE([BasicId], [HostURL], [ImagePathThumbnail], [ImagePathLarge]);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Images__IsActive__MakeId__ModelId]
    ON [dbo].[Con_EditCms_Images]([IsActive] ASC, [MakeId] ASC, [ModelId] ASC)
    INCLUDE([BasicId]);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Images__IsActive__IsMainImage]
    ON [dbo].[Con_EditCms_Images]([IsActive] ASC, [IsMainImage] ASC)
    INCLUDE([BasicId], [HostURL], [OriginalImgPath], [ImagePathCustom]);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Images_BasicId]
    ON [dbo].[Con_EditCms_Images]([BasicId] ASC, [ImageCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Images_BasicId_1]
    ON [dbo].[Con_EditCms_Images]([BasicId] ASC);

