CREATE TABLE [dbo].[TC_LeadInquiryType] (
    [TC_LeadInquiryTypeId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [LeadInquiryType]      VARCHAR (50) NULL,
    [IsActive]             BIT          CONSTRAINT [DF_TC_TC_LeadInquiryType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_LeadInquiryTypeId] PRIMARY KEY NONCLUSTERED ([TC_LeadInquiryTypeId] ASC)
);

