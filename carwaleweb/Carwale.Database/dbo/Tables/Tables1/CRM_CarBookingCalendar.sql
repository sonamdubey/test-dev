CREATE TABLE [dbo].[CRM_CarBookingCalendar] (
    [CBDID]               NUMERIC (18) NOT NULL,
    [BookingExpectedDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_CRM_CarBookingCalendar] PRIMARY KEY CLUSTERED ([CBDID] ASC)
);

