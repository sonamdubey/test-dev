CREATE TABLE [dbo].[TC_InquiryFollowUp] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [InquiryId]       NUMERIC (18)  NOT NULL,
    [Comments]        VARCHAR (500) NOT NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [NextFollowUp]    DATETIME      NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_TC_InquiryFollowUp_IsActive] DEFAULT ((1)) NOT NULL,
    [InquiryStatusId] INT           NULL,
    [ModifiedBy]      INT           NULL,
    [ModifiedDate]    DATETIME      NULL,
    [CustomerId]      BIGINT        NULL,
    CONSTRAINT [PK_TC_InquiryFollowUp] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_InquiryFollowUp__InquiryId__InquiryStatusId]
    ON [dbo].[TC_InquiryFollowUp]([InquiryId] ASC, [InquiryStatusId] ASC);

