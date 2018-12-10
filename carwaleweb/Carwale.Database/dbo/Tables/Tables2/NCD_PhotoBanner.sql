CREATE TABLE [dbo].[NCD_PhotoBanner] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT           NOT NULL,
    [ImageFull]       VARCHAR (100) NOT NULL,
    [ImageThumb]      VARCHAR (100) NOT NULL,
    [DirectoryPath]   VARCHAR (100) NOT NULL,
    [ImageRedirect]   TINYINT       NOT NULL,
    [ModelId]         INT           NULL,
    [IsActive]        BIT           CONSTRAINT [DF_NCD_PhotoBanner_IsActive] DEFAULT ((1)) NOT NULL,
    [HostUrl]         VARCHAR (250) NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF_NCD_PhotoBanner_IsReplicated] DEFAULT ((0)) NULL,
    [OriginalImgPath] VARCHAR (200) NULL,
    CONSTRAINT [PK_NCD_PhotoBanner] PRIMARY KEY CLUSTERED ([Id] ASC)
);

