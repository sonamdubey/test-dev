CREATE TABLE [dbo].[TC_FeedbackCalling_Inquiries] (
    [TC_FeedbackCalling_InquiriesId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesLeadId]             INT           NOT NULL,
    [EntryDate]                      DATETIME      NOT NULL,
    [TC_NewCarInquiriesId]           INT           NULL,
    [TC_InquirySourceId]             INT           NULL,
    [CreatedBy]                      INT           NULL,
    [TC_LeadDispositionId]           INT           NULL,
    [OldInquirySourceId]             INT           NULL,
    [OldDealerId]                    INT           NULL,
    [VersionId]                      INT           NULL,
    [DealerLeadDispositionId]        INT           NULL,
    [CityId]                         INT           NULL,
    [DealerSubDispositionId]         INT           NULL,
    [DealerDispositionDate]          DATETIME      NULL,
    [TC_LeadDispositionReason]       VARCHAR (200) NULL,
    [OriginalImgPath]                VARCHAR (250) NULL,
    [DMSScreenShotHostUrl]           VARCHAR (100) NULL,
    [DMSScreenShotStatusId]          INT           NULL,
    [DMSScreenShotUrl]               VARCHAR (100) NULL,
    CONSTRAINT [PK_TC_FeedbackCalling_Inquiries] PRIMARY KEY CLUSTERED ([TC_FeedbackCalling_InquiriesId] ASC)
);

