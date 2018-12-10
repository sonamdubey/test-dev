CREATE TABLE [dbo].[tc_carbooking_2011] (
    [TC_CarBookingId]    INT          IDENTITY (1, 1) NOT NULL,
    [TotalAmount]        DECIMAL (18) NOT NULL,
    [Discount]           DECIMAL (18) NOT NULL,
    [NetPayment]         DECIMAL (18) NOT NULL,
    [StockId]            NUMERIC (18) NOT NULL,
    [CustomerId]         NUMERIC (18) NOT NULL,
    [UserId]             INT          NOT NULL,
    [BookingDate]        DATETIME     NULL,
    [DeliveryDate]       DATE         NULL,
    [IsCompleted]        BIT          NOT NULL,
    [IsCanceled]         BIT          NOT NULL,
    [IsFinanceRequire]   BIT          NULL,
    [IsInsuranceRequire] BIT          NULL,
    [ModifiedDate]       DATETIME     NULL,
    [ModifiedBy]         INT          NULL
);

