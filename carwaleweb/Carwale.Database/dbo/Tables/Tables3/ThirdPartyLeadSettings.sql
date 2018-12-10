CREATE TABLE [dbo].[ThirdPartyLeadSettings] (
    [ThirdPartyLeadSettingId] INT           IDENTITY (1, 1) NOT NULL,
    [MakeId]                  NUMERIC (18)  NULL,
    [MakeName]                VARCHAR (30)  NOT NULL,
    [CampaignStartDate]       DATETIME      NOT NULL,
    [CampaignEndDate]         DATETIME      NOT NULL,
    [LeadVolume]              BIGINT        NULL,
    [LeadsSent]               INT           CONSTRAINT [DF_thirdparty_LeadsSent] DEFAULT ((0)) NOT NULL,
    [IsActive]                BIT           DEFAULT ((1)) NULL,
    [ModelId]                 NUMERIC (18)  NULL,
    [HttpRequestType]         TINYINT       NULL,
    [HttpRequestMessage]      VARCHAR (MAX) NULL,
    [Url]                     VARCHAR (500) NULL
);

