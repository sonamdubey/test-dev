CREATE TABLE [dbo].[LandingPageCampaign] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (100)  NOT NULL,
    [Type]              VARCHAR (150)  NOT NULL,
    [PrimaryHeading]    VARCHAR (200)  NOT NULL,
    [SecondaryHeading]  VARCHAR (300)  NULL,
    [IsEmailRequired]   BIT            NOT NULL,
    [DefaultModel]      INT            NULL,
    [ButtonText]        VARCHAR (100)  NULL,
    [TrailingText]      VARCHAR (2000) NULL,
    [IsActive]          BIT            NOT NULL,
    [CreatedOn]         DATETIME       NOT NULL,
    [CreatedBy]         INT            NOT NULL,
    [UpdatedOn]         DATETIME       NOT NULL,
    [UpdatedBy]         INT            NOT NULL,
    [IsDesktop]         BIT            DEFAULT ((0)) NOT NULL,
    [DesktopTemplateId] INT            NULL,
    [IsMobile]          BIT            DEFAULT ((0)) NOT NULL,
    [MobileTemplateId]  INT            NULL,
    CONSTRAINT [PK_LandingPageCampaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

