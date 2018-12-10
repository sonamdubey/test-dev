CREATE TABLE [dbo].[TC_OtherInquirySources] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [InquiryType] INT          NULL,
    [InquiryId]   INT          NULL,
    [SourceName]  VARCHAR (50) NULL,
    CONSTRAINT [PK_TC_OtherInquirySources] PRIMARY KEY CLUSTERED ([Id] ASC)
);

