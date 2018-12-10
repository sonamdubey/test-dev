CREATE TABLE [dbo].[Microsite_DealerModelBanners] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [DWModelId]       INT           NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [ImgPath]         VARCHAR (100) NULL,
    [ImgName]         VARCHAR (50)  NULL,
    [IsMainImg]       BIT           CONSTRAINT [DF_Microsite_DealerModelBanners_IsMainImg] DEFAULT ((0)) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Microsite_DealerModelBanners_IsActive] DEFAULT ((1)) NULL,
    [SortOrder]       INT           CONSTRAINT [DF_Microsite_DealerModelBanners_SortOrder] DEFAULT ((1)) NULL,
    [EntryDate]       DATETIME      CONSTRAINT [DF_Microsite_DealerModelBanners_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]    DATETIME      NULL,
    [DealerId]        INT           NULL,
    [OriginalImgPath] VARCHAR (250) NULL,
    [IsBanner]        BIT           NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

