CREATE TABLE [dbo].[TC_InquiriesFollowup] (
    [TC_FollowupId]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesLeadId]           BIGINT        NULL,
    [TC_UserId]                    INT           NULL,
    [FollowUpDate]                 DATETIME      NULL,
    [NextFollowupDate]             DATETIME      NULL,
    [Comment]                      VARCHAR (MAX) NULL,
    [TC_InquiryStatusId]           SMALLINT      NULL,
    [TC_InquiriesFollowupActionId] SMALLINT      NULL,
    [IsActive]                     BIT           NULL,
    [CustomerId]                   BIGINT        NULL,
    [BranchId]                     BIGINT        NULL,
    CONSTRAINT [PK_TC_Followup_Id] PRIMARY KEY NONCLUSTERED ([TC_FollowupId] ASC)
);

