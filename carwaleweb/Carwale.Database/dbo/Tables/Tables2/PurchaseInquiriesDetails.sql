CREATE TABLE [dbo].[PurchaseInquiriesDetails] (
    [PurchaseInquiryId] NUMERIC (18)  NOT NULL,
    [StartYear]         SMALLINT      NULL,
    [EndYear]           SMALLINT      NULL,
    [StartBudget]       NUMERIC (18)  NULL,
    [EndBudget]         NUMERIC (18)  NULL,
    [StartMileage]      NUMERIC (18)  NULL,
    [EndMileage]        NUMERIC (18)  NULL,
    [UpdateTimeStamp]   VARCHAR (100) NULL,
    CONSTRAINT [PK_PuchaseInqDetails] PRIMARY KEY CLUSTERED ([PurchaseInquiryId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PuchaseInqDetails_PurchaseInq] FOREIGN KEY ([PurchaseInquiryId]) REFERENCES [dbo].[PurchaseInquiries] ([ID]) ON UPDATE CASCADE
);

