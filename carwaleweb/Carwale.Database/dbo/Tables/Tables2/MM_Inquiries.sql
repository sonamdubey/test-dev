CREATE TABLE [dbo].[MM_Inquiries] (
    [MM_InquiriesId]       INT           IDENTITY (1, 1) NOT NULL,
    [BuyerMobile]          VARCHAR (20)  NULL,
    [CallStatus]           VARCHAR (20)  NULL,
    [CallStartDate]        DATETIME      NULL,
    [CallEndDate]          DATETIME      NULL,
    [CallDuration]         VARCHAR (80)  NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_MM_Inquiries_CreatedOn] DEFAULT (getdate()) NULL,
    [SellerMobile]         VARCHAR (100) NULL,
    [MaskingNumber]        VARCHAR (20)  NULL,
    [ConsumerId]           INT           NULL,
    [ConsumerType]         TINYINT       NULL,
    [TC_RecordId]          INT           NULL,
    [IsPushedToTC]         BIT           NULL,
    [PushedOn]             DATETIME      NULL,
    [CallId]               VARCHAR (60)  NULL,
    [RecordingURL]         VARCHAR (200) NULL,
    [ReceivedSellerNumber] VARCHAR (20)  NULL,
    [ProductTypeId]        INT           NULL,
    [LeadCampaignId]       INT           NULL,
    [CallerCircle]         VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([MM_InquiriesId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_MM_Inquiries_ConsumerId]
    ON [dbo].[MM_Inquiries]([ConsumerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MM_Inquiries_CallStartDate]
    ON [dbo].[MM_Inquiries]([CallStartDate] ASC)
    INCLUDE([ConsumerId]);

