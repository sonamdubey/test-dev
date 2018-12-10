CREATE TABLE [dbo].[PQ_DealerSponsored] (
    [Id]                     INT           IDENTITY (1, 1) NOT NULL,
    [ModelId]                INT           NULL,
    [CityId]                 INT           NULL,
    [DealerId]               INT           NOT NULL,
    [DealerName]             VARCHAR (80)  NOT NULL,
    [Phone]                  VARCHAR (50)  NOT NULL,
    [IsActive]               BIT           NOT NULL,
    [DealerEmailId]          VARCHAR (250) NULL,
    [StartDate]              DATE          NULL,
    [EndDate]                DATE          NULL,
    [ZoneId]                 INT           NULL,
    [UpdatedBy]              INT           NULL,
    [UpdatedOn]              DATETIME      NULL,
    [DealerLeadBusinessType] SMALLINT      CONSTRAINT [DF_PQ_DealerSponsored_DealerLeadBusinessType] DEFAULT ((0)) NULL,
    [IsDesktop]              BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsDesktop] DEFAULT ((0)) NOT NULL,
    [IsMobile]               BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsMobile] DEFAULT ((0)) NOT NULL,
    [IsAndroid]              BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsAndroid] DEFAULT ((0)) NOT NULL,
    [IsIPhone]               BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsIPhone] DEFAULT ((0)) NOT NULL,
    [CampaignBehaviour]      INT           NULL,
    [TotalGoal]              INT           NULL,
    [DailyGoal]              INT           NULL,
    [TotalCount]             INT           CONSTRAINT [DF_PQ_DealerSponsored_TotalCount] DEFAULT ((0)) NULL,
    [DailyCount]             INT           CONSTRAINT [DF_PQ_DealerSponsored_DailyCount] DEFAULT ((0)) NULL,
    [LeadPanel]              TINYINT       NULL,
    [EnableUserEmail]        BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsMail] DEFAULT ((0)) NULL,
    [EnableUserSMS]          BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsSMS] DEFAULT ((0)) NULL,
    [CampaignPriority]       SMALLINT      NULL,
    [LinkText]               VARCHAR (250) NULL,
    [IsMailerSent]           BIT           CONSTRAINT [DF_PQ_DealerSponsored_IsMailerSent] DEFAULT ((0)) NULL,
    [EnableDealerEmail]      BIT           NULL,
    [EnableDealerSMS]        BIT           NULL,
    [CostPerLead]            FLOAT (53)    NULL,
    [ShowEmail]              BIT           CONSTRAINT [DF_PQ_dealersponsored_ShowEmail] DEFAULT ((0)) NULL,
    [PausedDate]             DATETIME      NULL,
    [IsDefaultNumber]        INT           NULL,
    [NotificationMobile]     VARCHAR (100) NULL,
    [IsThirdPartyCampaign]   BIT           DEFAULT ((1)) NOT NULL,
    [isFeaturedEnabled]      BIT           DEFAULT ((1)) NULL,
    [ShowInRecommendation]   BIT           DEFAULT ((1)) NULL,
    CONSTRAINT [PK_PQ_DealerSponsored] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PQ_DealerSponsored_DealerId]
    ON [dbo].[PQ_DealerSponsored]([DealerId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-No Limit, 2-Lead Wise 3- Expsoure Wise', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PQ_DealerSponsored', @level2type = N'COLUMN', @level2name = N'CampaignBehaviour';

