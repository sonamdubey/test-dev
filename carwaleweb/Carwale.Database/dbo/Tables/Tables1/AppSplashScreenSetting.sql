CREATE TABLE [dbo].[AppSplashScreenSetting] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CampaignName]    VARCHAR (50)  NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [DirPath]         VARCHAR (50)  NULL,
    [ImageName]       VARCHAR (200) NULL,
    [PlatformId]      INT           NULL,
    [StartDate]       DATE          NULL,
    [EndDate]         DATE          NULL,
    [IsActive]        INT           NULL,
    [ModifiedBy]      INT           NULL,
    [ModifiedOn]      DATETIME      NULL,
    [OriginalImgPath] VARCHAR (150) NULL,
    [IsReplicated]    BIT           NULL
);

