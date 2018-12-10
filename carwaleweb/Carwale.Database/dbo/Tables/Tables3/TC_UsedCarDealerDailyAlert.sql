CREATE TABLE [dbo].[TC_UsedCarDealerDailyAlert] (
    [TC_UsersId]               INT     NULL,
    [CurrentDayInquiryCount]   INT     NULL,
    [CurrentMonthInquiryCount] INT     NULL,
    [CurrentDayBookingCount]   INT     NULL,
    [CurrentMonthBookingCount] INT     NULL,
    [CurrentDayLostCount]      INT     NULL,
    [CurrentMonthLostCount]    INT     NULL,
    [PendingFollowup]          INT     NULL,
    [TomorrowFollowup]         INT     NULL,
    [TC_LeadInquiryTypeId]     TINYINT NULL
);

