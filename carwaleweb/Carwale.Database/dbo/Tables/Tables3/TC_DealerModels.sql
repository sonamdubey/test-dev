CREATE TABLE [dbo].[TC_DealerModels] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [CWModelId]       INT           NOT NULL,
    [DWModelName]     VARCHAR (50)  NOT NULL,
    [DealerId]        INT           NOT NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [ImgPath]         VARCHAR (100) NULL,
    [ImgName]         VARCHAR (50)  NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_TC_DealerModels_IsDeleted] DEFAULT ((0)) NULL,
    [EntryDate]       DATETIME      CONSTRAINT [DF_TC_DealerModels_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]    DATETIME      NULL,
    [DWBodyStyleId]   INT           NULL,
    [OriginalImgPath] VARCHAR (250) NULL,
    CONSTRAINT [PK__TC_Deale__3214EC271999B95B] PRIMARY KEY CLUSTERED ([ID] ASC)
);

