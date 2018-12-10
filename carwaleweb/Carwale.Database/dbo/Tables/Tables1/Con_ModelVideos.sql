CREATE TABLE [dbo].[Con_ModelVideos] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [ModelId]          INT           NULL,
    [VideoSrc]         VARCHAR (500) NULL,
    [isActive]         BIT           NULL,
    [Entrydate]        DATETIME      NULL,
    [VideoTitle]       VARCHAR (500) NULL,
    [ThumbnailHostURL] VARCHAR (50)  NULL,
    [ThumbnailDirPath] VARCHAR (20)  NULL,
    [ThumbnailImage]   VARCHAR (20)  NULL,
    [OriginalImgPath]  VARCHAR (150) NULL,
    [IsReplicated]     BIT           NULL
);

