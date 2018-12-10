CREATE TABLE [dbo].[AutoExpo_ImageDetails] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Title]           VARCHAR (150)  NULL,
    [ImageUrl]        VARCHAR (500)  NULL,
    [ImageLink]       VARCHAR (500)  NULL,
    [Description]     VARCHAR (MAX)  NULL,
    [IsActive]        BIT            CONSTRAINT [DF_AutoExpo_ImageDetails_IsActive] DEFAULT ((1)) NULL,
    [MakeId]          INT            NULL,
    [ImageCategoryId] INT            NULL,
    [Priority]        INT            NULL,
    [CreatedOn]       DATETIME       CONSTRAINT [DF_AutoExpo_ImageDetails_CreatedOn] DEFAULT (getdate()) NULL,
    [ApplicationId]   INT            NULL,
    [PlatformType]    INT            NULL,
    [ThumbImageUrl]   VARCHAR (500)  NULL,
    [IsVideo]         BIT            CONSTRAINT [DF_AutoExpo_ImageDetails_IsVideo] DEFAULT ((0)) NULL,
    [CssClass]        VARCHAR (100)  NULL,
    [AndroidLink]     VARCHAR (1000) NULL,
    CONSTRAINT [PK_AutoExpo_ImageDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

