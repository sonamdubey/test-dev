CREATE TABLE [dbo].[TC_CarBooking] (
    [TC_CarBookingId]    INT          IDENTITY (1, 1) NOT NULL,
    [TotalAmount]        DECIMAL (18) NOT NULL,
    [Discount]           DECIMAL (18) NOT NULL,
    [NetPayment]         DECIMAL (18) NOT NULL,
    [StockId]            NUMERIC (18) NOT NULL,
    [CustomerId]         NUMERIC (18) NOT NULL,
    [UserId]             INT          NOT NULL,
    [BookingDate]        DATETIME     NULL,
    [DeliveryDate]       DATE         NULL,
    [IsCompleted]        BIT          CONSTRAINT [DF_TC_CarBooking_IsComplete] DEFAULT ((0)) NOT NULL,
    [IsCanceled]         BIT          CONSTRAINT [DF_TC_CarBooking_IsCanceled] DEFAULT ((0)) NOT NULL,
    [IsFinanceRequire]   BIT          DEFAULT ((0)) NULL,
    [IsInsuranceRequire] BIT          DEFAULT ((0)) NULL,
    [ModifiedDate]       DATETIME     NULL,
    [ModifiedBy]         INT          NULL,
    [Old_CustomerId]     BIGINT       NULL,
    CONSTRAINT [PK_TC_CarBooking] PRIMARY KEY CLUSTERED ([TC_CarBookingId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'checking for payment finished for carbooking', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_CarBooking', @level2type = N'COLUMN', @level2name = N'IsCompleted';

