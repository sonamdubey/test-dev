CREATE TABLE [dbo].[TC_DealerModels_Bkp15082015] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [CWModelId]       INT           NOT NULL,
    [DWModelName]     VARCHAR (50)  NOT NULL,
    [DealerId]        INT           NOT NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [ImgPath]         VARCHAR (100) NULL,
    [ImgName]         VARCHAR (50)  NULL,
    [IsDeleted]       BIT           NULL,
    [EntryDate]       DATETIME      NULL,
    [ModifiedDate]    DATETIME      NULL,
    [DWBodyStyleId]   INT           NULL,
    [OriginalImgPath] VARCHAR (250) NULL
);

