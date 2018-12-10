CREATE TABLE [dbo].[TC_InquiriesLead] (
    [TC_InquiriesLeadId]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]                     NUMERIC (18)  NULL,
    [TC_CustomerId]                BIGINT        NULL,
    [TC_UserId]                    BIGINT        NULL,
    [InquiryCount]                 SMALLINT      NULL,
    [NextFollowUpDate]             DATETIME      NULL,
    [LastFollowUpDate]             DATETIME      NULL,
    [LastFollowUpComment]          VARCHAR (MAX) NULL,
    [TC_InquiryTypeId]             SMALLINT      NULL,
    [TC_InquiryStatusId]           SMALLINT      NULL,
    [TC_InquiriesFollowupActionId] SMALLINT      NULL,
    [CreatedBy]                    BIGINT        NULL,
    [CreatedDate]                  DATETIME      NULL,
    [ModifiedBy]                   BIGINT        NULL,
    [ModifiedDate]                 DATETIME      NULL,
    [IsActionTaken]                BIT           CONSTRAINT [DF_TC_InquiriesLead_IsActionTaken] DEFAULT ((0)) NULL,
    [TC_LeadTypeId]                TINYINT       NULL,
    [InqTypeDesc]                  VARCHAR (100) NULL,
    [IsActive]                     BIT           DEFAULT ((1)) NULL,
    [Old_TC_CustomerId]            BIGINT        NULL,
    [TC_LeadId]                    INT           NULL,
    [TC_LeadInquiryTypeId]         TINYINT       NULL,
    [TC_LeadStageId]               TINYINT       NULL,
    [TC_LeadDispositionID]         TINYINT       NULL,
    [CarDetails]                   VARCHAR (MAX) NULL,
    [LatestInquiryDate]            DATETIME      NULL,
    [InqSourceId]                  SMALLINT      NULL,
    [TC_CampaignSchedulingId]      INT           NULL,
    [LatestVersionId]              INT           NULL,
    [CampaignId]                   INT           NULL,
    [ContractId]                   INT           NULL,
    [TC_BWLeadStatusId]            TINYINT       NULL,
    [RegistrationNumber]           VARCHAR (50)  NULL,
    CONSTRAINT [PK_TC_InquiryLead_Id] PRIMARY KEY CLUSTERED ([TC_InquiriesLeadId] ASC),
    CONSTRAINT [DF_TC_inquirieslead_TC_LeadDisposition] FOREIGN KEY ([TC_LeadDispositionID]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_tc_inquirieslead_TC_LeadInquiryType] FOREIGN KEY ([TC_LeadInquiryTypeId]) REFERENCES [dbo].[TC_LeadInquiryType] ([TC_LeadInquiryTypeId]),
    CONSTRAINT [DF_TC_tc_inquirieslead_TC_LeadStage] FOREIGN KEY ([TC_LeadStageId]) REFERENCES [dbo].[TC_LeadStage] ([TC_LeadStageId])
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_LeadId_TC_InquiriesLead]
    ON [dbo].[TC_InquiriesLead]([TC_LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_InquiriesLead_TC_InquiryStatusId]
    ON [dbo].[TC_InquiriesLead]([TC_InquiryStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_InquiriesLead_TC_InquiriesFollowupActionId]
    ON [dbo].[TC_InquiriesLead]([TC_InquiriesFollowupActionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_InquiriesLead_TC_CustomerId]
    ON [dbo].[TC_InquiriesLead]([TC_CustomerId] ASC, [TC_LeadTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_InquiriesLead_BranchId]
    ON [dbo].[TC_InquiriesLead]([BranchId] ASC, [TC_LeadStageId] ASC, [TC_LeadDispositionID] ASC)
    INCLUDE([TC_InquiryStatusId], [TC_LeadId], [CarDetails], [LatestInquiryDate]);


GO
CREATE NONCLUSTERED INDEX [IX_InquiriesLead_TC_UserId]
    ON [dbo].[TC_InquiriesLead]([TC_UserId] ASC, [TC_LeadDispositionID] ASC)
    INCLUDE([TC_InquiriesLeadId]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_InquiriesLead_TC_LeadInquiryTypeId]
    ON [dbo].[TC_InquiriesLead]([BranchId] ASC, [TC_LeadInquiryTypeId] ASC)
    INCLUDE([TC_LeadId]);

