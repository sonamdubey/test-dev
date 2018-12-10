CREATE TABLE [dbo].[Con_EditCms_Images_06082012] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [BasicId]            NUMERIC (18)  NULL,
    [ImageCategoryId]    NUMERIC (18)  NULL,
    [Caption]            VARCHAR (250) NULL,
    [IsActive]           BIT           NULL,
    [LastUpdatedTime]    DATETIME      NULL,
    [LastUpdatedBy]      NUMERIC (18)  NULL,
    [Sequence]           INT           NULL,
    [RTImageId]          INT           NULL,
    [IsUpdated]          BIT           NULL,
    [IsReplicated]       BIT           NULL,
    [HostURL]            VARCHAR (100) NULL,
    [HasCustomImg]       BIT           NULL,
    [MakeId]             INT           NULL,
    [ModelId]            INT           NULL,
    [IsMainImage]        BIT           NULL,
    [ImageName]          VARCHAR (100) NULL,
    [ImagePathThumbnail] VARCHAR (100) NULL,
    [ImagePathLarge]     VARCHAR (100) NULL,
    [ImagePathOriginal]  VARCHAR (100) NULL,
    [ImagePathCustom]    VARCHAR (100) NULL
);

