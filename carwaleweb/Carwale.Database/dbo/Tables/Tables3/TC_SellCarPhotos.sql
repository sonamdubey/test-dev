CREATE TABLE [dbo].[TC_SellCarPhotos] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TC_SellerInquiriesId] BIGINT        NOT NULL,
    [ImageUrlFull]         VARCHAR (180) NULL,
    [ImageUrlThumb]        VARCHAR (180) NULL,
    [ImageUrlThumbSmall]   VARCHAR (180) NULL,
    [DirectoryPath]        VARCHAR (200) NULL,
    [IsMain]               BIT           NOT NULL,
    [IsActive]             BIT           CONSTRAINT [DF_TC_SellCarPhotos_IsActive] DEFAULT ((1)) NOT NULL,
    [HostUrl]              VARCHAR (100) NULL,
    [IsReplicated]         BIT           CONSTRAINT [DF_TC_SellCarPhotos_IsReplicated] DEFAULT ((0)) NULL,
    [EntryDate]            DATETIME      CONSTRAINT [DF_TC_SellCarPhotos_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]         DATETIME      NULL,
    [ModifiedBy]           INT           NULL,
    [StatusId]             INT           DEFAULT ((1)) NULL,
    [OriginalImgPath]      VARCHAR (250) NULL,
    CONSTRAINT [PK_TC_SellCarPhotos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

