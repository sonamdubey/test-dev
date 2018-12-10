CREATE TABLE [dbo].[NCD_Inquiries] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]             INT           NOT NULL,
    [CustomerId]           INT           NOT NULL,
    [CityId]               INT           NULL,
    [VersionId]            INT           NULL,
    [BuyPlan]              VARCHAR (30)  NULL,
    [RequestType]          TINYINT       NOT NULL,
    [InquiryDescription]   TEXT          NULL,
    [EntryDate]            DATETIME      NOT NULL,
    [InquirySource]        TINYINT       NOT NULL,
    [IsActive]             BIT           CONSTRAINT [DF_NCD_Inquiries_IsActive] DEFAULT ((1)) NOT NULL,
    [NextCallTime]         DATETIME      NULL,
    [LeadStatusId]         TINYINT       NULL,
    [LatestActionDesc]     VARCHAR (500) NULL,
    [IsActionTaken]        BIT           CONSTRAINT [DF_NCD_Inquiries_IsActionTaken] DEFAULT ((0)) NULL,
    [IsAccepted]           BIT           CONSTRAINT [DF_NCD_Inquiries_IsAccepted] DEFAULT ((1)) NULL,
    [RejectionId]          INT           NULL,
    [RejectionComment]     VARCHAR (200) NULL,
    [IsReclaimReplacement] BIT           CONSTRAINT [DF_NCD_Inquiries_IsReclaimReplacement] DEFAULT ((0)) NULL,
    [AssignedExecutive]    INT           NULL,
    [TCQuoteId]            NUMERIC (18)  NULL,
    [UpdatedOn]            DATETIME      NULL,
    [CWCustomerId]         NUMERIC (18)  NULL,
    [WSReturn]             VARCHAR (100) NULL,
    [CampaignId]           NUMERIC (18)  NULL,
    CONSTRAINT [PK_NCD_Inquiries] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_NCD_Inquiries__DealerId__RequestType__IsActive__IsActionTaken__AssignedExecutive]
    ON [dbo].[NCD_Inquiries]([DealerId] ASC, [RequestType] ASC, [IsActive] ASC, [IsActionTaken] ASC, [AssignedExecutive] ASC)
    INCLUDE([CustomerId]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=NCD,2=CarWale', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_Inquiries', @level2type = N'COLUMN', @level2name = N'InquirySource';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'if lead is rejected. In some cases it might come for replacement. This flag will be true id requested for replacement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_Inquiries', @level2type = N'COLUMN', @level2name = N'IsReclaimReplacement';

