CREATE TABLE [dbo].[NCD_LeadRequest] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [InquiryId]      INT      NOT NULL,
    [Inq_FollowupId] INT      NOT NULL,
    [RequestType]    INT      NOT NULL,
    [FollowupDate]   DATETIME NOT NULL,
    CONSTRAINT [PK_NCD_LeadRequest] PRIMARY KEY CLUSTERED ([Id] ASC)
);

