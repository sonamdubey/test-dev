CREATE TABLE [dbo].[TC_NewCarPaymentDetails] (
    [TC_NewCarBookingId] INT          NULL,
    [BookingAmount]      DECIMAL (18) NULL,
    [ReceiptNo]          VARCHAR (15) NULL,
    [PaymentDate]        DATETIME     NULL,
    [PaymentMode]        VARCHAR (10) NULL,
    [DraftNo]            VARCHAR (20) NULL,
    [ChequeNo]           VARCHAR (20) NULL
);

