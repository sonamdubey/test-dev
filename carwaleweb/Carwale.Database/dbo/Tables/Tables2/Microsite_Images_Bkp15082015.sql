CREATE TABLE [dbo].[Microsite_Images_Bkp15082015] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]              INT           NOT NULL,
    [ThumbImage]            VARCHAR (50)  NULL,
    [LargeImage]            VARCHAR (50)  NULL,
    [EntryDate]             DATETIME      NOT NULL,
    [DirectoryPath]         VARCHAR (100) NULL,
    [HostURL]               VARCHAR (50)  NULL,
    [IsActive]              BIT           NOT NULL,
    [IsReplicated]          BIT           NOT NULL,
    [IsBanner]              BIT           NOT NULL,
    [BannerImgSortingOrder] SMALLINT      NULL,
    [LandingURL]            VARCHAR (50)  NULL,
    [ModelId]               INT           NULL,
    [ImageCategoryId]       TINYINT       NULL,
    [StatusId]              INT           NULL,
    [ImgPathCustom600]      VARCHAR (100) NULL,
    [ImgPathCustom300]      VARCHAR (100) NULL,
    [IsResize]              BIT           NULL
);

