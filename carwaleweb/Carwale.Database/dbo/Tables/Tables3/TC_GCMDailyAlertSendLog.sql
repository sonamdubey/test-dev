CREATE TABLE [dbo].[TC_GCMDailyAlertSendLog] (
    [TC_UsersId]               INT            NULL,
    [CurrentDayInquiryCount]   INT            NULL,
    [CurrentMonthInquiryCount] INT            NULL,
    [CurrentDayBookingCount]   INT            NULL,
    [CurrentMonthBookingCount] INT            NULL,
    [CurrentDayLostCount]      INT            NULL,
    [CurrentMonthLostCount]    INT            NULL,
    [PendingFollowup]          INT            NULL,
    [TomorrowFollowup]         INT            NULL,
    [TC_LeadInquiryTypeId]     TINYINT        NULL,
    [SendTime]                 DATETIME       NULL,
    [GCMRegistrationId]        VARCHAR (8000) NULL
);

