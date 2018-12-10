CREATE TABLE [dbo].[Microsite_DealerModelBanners_Bkp15082015] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [DWModelId]       INT           NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [ImgPath]         VARCHAR (100) NULL,
    [ImgName]         VARCHAR (50)  NULL,
    [IsMainImg]       BIT           NULL,
    [IsActive]        BIT           NULL,
    [SortOrder]       INT           NULL,
    [EntryDate]       DATETIME      NULL,
    [ModifiedDate]    DATETIME      NULL,
    [DealerId]        INT           NULL,
    [OriginalImgPath] VARCHAR (250) NULL
);

