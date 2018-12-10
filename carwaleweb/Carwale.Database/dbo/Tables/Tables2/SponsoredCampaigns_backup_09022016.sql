CREATE TABLE [dbo].[SponsoredCampaigns_backup_09022016] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ModelId]            NUMERIC (18)  NULL,
    [AdScript]           VARCHAR (MAX) NULL,
    [IsActive]           BIT           NULL,
    [CampaignCategoryId] TINYINT       NULL,
    [IsDeleted]          BIT           NULL,
    [StartDate]          DATETIME      NULL,
    [EndDate]            DATETIME      NULL,
    [SponsoredTitle]     VARCHAR (25)  NULL,
    [IsSponsored]        BIT           NULL,
    [IsDefault]          BIT           NULL,
    [PlatformId]         INT           NULL,
    [CreatedBy]          INT           NULL,
    [CreatedOn]          DATETIME      NULL,
    [UpdatedBy]          INT           NULL,
    [UpdatedOn]          DATETIME      NULL,
    [ImageUrl]           VARCHAR (300) NULL,
    [JumbotronPos]       SMALLINT      NULL,
    [VPosition]          VARCHAR (10)  NULL,
    [HPosition]          VARCHAR (10)  NULL,
    [BackGroundColor]    VARCHAR (10)  NULL
);

