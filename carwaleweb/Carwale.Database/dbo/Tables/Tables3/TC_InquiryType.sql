CREATE TABLE [dbo].[TC_InquiryType] (
    [TC_InquiryTypeId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [InquiryType]      VARCHAR (20) NULL,
    [IsActive]         BIT          NULL,
    [TC_LeadTypeId]    TINYINT      NULL,
    [abbreviation]     VARCHAR (5)  NULL,
    CONSTRAINT [PK_TC_InquiryType] PRIMARY KEY CLUSTERED ([TC_InquiryTypeId] ASC)
);

