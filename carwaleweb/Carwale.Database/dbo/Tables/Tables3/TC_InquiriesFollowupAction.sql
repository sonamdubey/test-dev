CREATE TABLE [dbo].[TC_InquiriesFollowupAction] (
    [TC_InquiriesFollowupActionId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [ActionName]                   VARCHAR (50) NOT NULL,
    [InquiryType]                  INT          NULL,
    [isCommon]                     BIT          NULL,
    [isActive]                     BIT          CONSTRAINT [DF_TC_InquiriesFollowupAction_isActive] DEFAULT ((1)) NOT NULL,
    [isConnected]                  BIT          NULL,
    [IsConverted]                  BIT          NULL,
    [IsClosed]                     BIT          NULL,
    [TC_LeadTypeId]                TINYINT      NULL,
    CONSTRAINT [PK_TC_FollowupAction_Id] PRIMARY KEY NONCLUSTERED ([TC_InquiriesFollowupActionId] ASC)
);

