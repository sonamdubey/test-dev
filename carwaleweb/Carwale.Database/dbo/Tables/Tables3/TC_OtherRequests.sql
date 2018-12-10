CREATE TABLE [dbo].[TC_OtherRequests] (
    [TC_OtherRequestID] NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId]    INT           NULL,
    [Comments]          VARCHAR (250) NULL,
    [TC_InquiryTypeId]  TINYINT       DEFAULT (NULL) NULL,
    [TC_CustomerId]     BIGINT        DEFAULT (NULL) NULL,
    [InquirySourceId]   TINYINT       DEFAULT (NULL) NULL,
    [CreatedDate]       DATETIME      DEFAULT (NULL) NULL,
    [PreferredDateTime] DATETIME      NULL,
    CONSTRAINT [PK_TC_OtherRequests] PRIMARY KEY CLUSTERED ([TC_OtherRequestID] ASC)
);

