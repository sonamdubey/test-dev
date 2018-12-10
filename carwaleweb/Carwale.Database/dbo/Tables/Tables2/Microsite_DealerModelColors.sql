CREATE TABLE [dbo].[Microsite_DealerModelColors] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]        NUMERIC (18)  NOT NULL,
    [DWModelId]       NUMERIC (18)  NOT NULL,
    [ColorName]       VARCHAR (50)  NOT NULL,
    [HostUrl]         VARCHAR (200) NULL,
    [ImgPath]         VARCHAR (100) NULL,
    [ImgName]         VARCHAR (50)  NULL,
    [ColorCode]       VARCHAR (20)  NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Microsite_DealerModelColors_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]       DATETIME      CONSTRAINT [DF_Microsite_DealerModelColors_EntryDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]    DATETIME      NULL,
    [OriginalImgPath] VARCHAR (250) NULL,
    CONSTRAINT [PK_Microsite_DealerModelColors] PRIMARY KEY CLUSTERED ([Id] ASC)
);

