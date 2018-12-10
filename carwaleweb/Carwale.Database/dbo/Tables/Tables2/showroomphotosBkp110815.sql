CREATE TABLE [dbo].[showroomphotosBkp110815] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]        NUMERIC (18)  NOT NULL,
    [Thumbnail]       VARCHAR (50)  NOT NULL,
    [LargeImage]      VARCHAR (50)  NOT NULL,
    [ImageCategory]   TINYINT       NOT NULL,
    [isMainPhoto]     BIT           NOT NULL,
    [isActive]        BIT           NOT NULL,
    [IsReplicated]    BIT           NULL,
    [HostURL]         VARCHAR (100) NULL,
    [DirectoryPath]   VARCHAR (100) NULL,
    [RequestDate]     DATETIME      NULL,
    [OriginalImgPath] VARCHAR (250) NULL
);

