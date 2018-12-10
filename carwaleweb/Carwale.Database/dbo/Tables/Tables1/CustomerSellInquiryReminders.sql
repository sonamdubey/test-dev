CREATE TABLE [dbo].[CustomerSellInquiryReminders] (
    [CustSellInqId]    NUMERIC (18) NOT NULL,
    [LastReminderDate] DATETIME     NULL,
    [LastCallDate]     DATETIME     NULL,
    [LastReportDate]   DATETIME     NULL,
    CONSTRAINT [PK_CustomerSellInquiryReminders] PRIMARY KEY CLUSTERED ([CustSellInqId] ASC) WITH (FILLFACTOR = 90)
);

