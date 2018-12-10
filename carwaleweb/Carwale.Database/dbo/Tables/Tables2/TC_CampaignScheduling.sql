CREATE TABLE [dbo].[TC_CampaignScheduling] (
    [TC_CampaignSchedulingId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_MainCampaignId]       INT           NULL,
    [TC_SubCampaignId]        INT           NULL,
    [CampaignName]            VARCHAR (200) NULL,
    [CampaignFromDate]        DATETIME      NULL,
    [CampaignToDate]          DATETIME      NULL,
    [CityId]                  INT           NULL,
    [Amount]                  INT           NULL,
    [LeadTarget]              INT           NULL,
    [BranchId]                BIGINT        NULL,
    [UserId]                  INT           NULL,
    [EntryDate]               DATETIME      NULL,
    [ModifiedBy]              INT           NULL,
    [ModifiedDate]            DATETIME      NULL,
    [IsActive]                BIT           NULL,
    [IsSpecialUser]           BIT           NULL,
    [ModelId]                 INT           NULL,
    [Details]                 VARCHAR (MAX) NULL,
    CONSTRAINT [PK_TC_CampaignScheduling] PRIMARY KEY CLUSTERED ([TC_CampaignSchedulingId] ASC)
);

