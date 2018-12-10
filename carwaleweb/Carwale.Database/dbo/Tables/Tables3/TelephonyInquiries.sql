CREATE TABLE [dbo].[TelephonyInquiries] (
    [CustomerId]       NUMERIC (18)   NOT NULL,
    [InquiryTypeId]    INT            NOT NULL,
    [CustomerComments] VARCHAR (2000) NULL,
    [LastUpdated]      DATETIME       NOT NULL,
    CONSTRAINT [PK_TelephonyInquiries] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [InquiryTypeId] ASC) WITH (FILLFACTOR = 90)
);

