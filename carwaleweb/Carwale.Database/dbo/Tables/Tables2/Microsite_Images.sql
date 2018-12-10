CREATE TABLE [dbo].[Microsite_Images] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]              INT           NOT NULL,
    [ThumbImage]            VARCHAR (50)  NULL,
    [LargeImage]            VARCHAR (50)  NULL,
    [EntryDate]             DATETIME      NOT NULL,
    [DirectoryPath]         VARCHAR (100) NULL,
    [HostURL]               VARCHAR (50)  NULL,
    [IsActive]              BIT           CONSTRAINT [DF_MicroSites_Images_IsActive] DEFAULT ((1)) NOT NULL,
    [IsReplicated]          BIT           CONSTRAINT [DF_MicroSites_Images_IsReplicated] DEFAULT ((0)) NOT NULL,
    [IsBanner]              BIT           CONSTRAINT [DF_MicroSites_Images_IsBanner] DEFAULT ((0)) NOT NULL,
    [BannerImgSortingOrder] SMALLINT      NULL,
    [LandingURL]            VARCHAR (50)  NULL,
    [ModelId]               INT           NULL,
    [ImageCategoryId]       TINYINT       NULL,
    [StatusId]              INT           CONSTRAINT [DF_Microsite_Images_StatusId] DEFAULT ((1)) NULL,
    [ImgPathCustom600]      VARCHAR (100) NULL,
    [ImgPathCustom300]      VARCHAR (100) NULL,
    [IsResize]              BIT           NULL,
    [OriginalImgPath]       VARCHAR (200) NULL,
    [IsMainBanner]          BIT           DEFAULT ((0)) NOT NULL,
    [isDeleted]             BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MicroSites_Images] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_Microsite_Images_DealerId]
    ON [dbo].[Microsite_Images]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Microsite_Images_IsActive]
    ON [dbo].[Microsite_Images]([DealerId] ASC, [IsActive] ASC, [IsBanner] ASC, [BannerImgSortingOrder] ASC);

