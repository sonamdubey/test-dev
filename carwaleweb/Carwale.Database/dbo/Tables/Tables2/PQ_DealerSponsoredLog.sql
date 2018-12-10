﻿CREATE TABLE [dbo].[PQ_DealerSponsoredLog] (
    [PQ_DealerSponsoredId]   INT           NOT NULL,
    [DealerId]               NUMERIC (18)  NOT NULL,
    [DealerName]             VARCHAR (50)  NOT NULL,
    [Phone]                  VARCHAR (50)  NOT NULL,
    [IsActive]               BIT           NOT NULL,
    [DealerEmailId]          VARCHAR (250) NULL,
    [StartDate]              DATE          NULL,
    [EndDate]                DATE          NULL,
    [ActionTakenBy]          INT           NULL,
    [ActionTakenOn]          DATETIME      NULL,
    [DealerLeadBusinessType] SMALLINT      NULL,
    [IsDesktop]              BIT           NOT NULL,
    [IsMobile]               BIT           NOT NULL,
    [IsAndroid]              BIT           NOT NULL,
    [IsIPhone]               BIT           NOT NULL,
    [CampaignBehaviour]      INT           NULL,
    [TotalGoal]              INT           NULL,
    [DailyGoal]              INT           NULL,
    [AssignedTemplateId]     NUMERIC (18)  NULL,
    [LeadPanel]              TINYINT       NULL,
    [EnableUserEmail]        BIT           NULL,
    [EnableUserSMS]          BIT           NULL,
    [CampaignPriority]       INT           NULL,
    [Remarks]                NVARCHAR (50) NOT NULL,
    [LinkText]               VARCHAR (250) NULL,
    [EnableDealerEmail]      BIT           NULL,
    [EnableDealerSMS]        BIT           NULL,
    [LastTotalCount]         INT           NULL,
    [LastDailyCount]         INT           NULL,
    [CostPerLead]            FLOAT (53)    NULL,
    [NotificationMobile]     VARCHAR (100) NULL,
    [IsThirdPartyCampaign]   BIT           NULL,
    [isFeaturedEnabled]      BIT           DEFAULT ((1)) NULL
);

