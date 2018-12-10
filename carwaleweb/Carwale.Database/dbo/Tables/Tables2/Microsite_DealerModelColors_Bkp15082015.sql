CREATE TABLE [dbo].[Microsite_DealerModelColors_Bkp15082015] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]        NUMERIC (18)  NOT NULL,
    [DWModelId]       NUMERIC (18)  NOT NULL,
    [ColorName]       VARCHAR (50)  NOT NULL,
    [HostUrl]         VARCHAR (200) NULL,
    [ImgPath]         VARCHAR (100) NULL,
    [ImgName]         VARCHAR (50)  NULL,
    [ColorCode]       VARCHAR (20)  NULL,
    [IsActive]        BIT           NOT NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [ModifiedDate]    DATETIME      NULL,
    [OriginalImgPath] VARCHAR (250) NULL
);

