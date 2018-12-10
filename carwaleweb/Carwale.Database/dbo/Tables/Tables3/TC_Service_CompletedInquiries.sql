CREATE TABLE [dbo].[TC_Service_CompletedInquiries] (
    [TC_Service_CompletedInquriesId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_Service_InquiriesId]         INT           NULL,
    [Amount]                         INT           NULL,
    [NextServiceKms]                 INT           NULL,
    [NextServiceDate]                DATETIME      NULL,
    [Comments]                       VARCHAR (200) NULL,
    [invoiceKey]                     VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([TC_Service_CompletedInquriesId] ASC)
);

