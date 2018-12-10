CREATE TABLE [dbo].[FOB_BookingRefund] (
    [id]                NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [BRId]              NUMERIC (18)    NULL,
    [Amount]            NUMERIC (18, 2) NULL,
    [RefundDate]        DATETIME        NULL,
    [RefundRequestDate] DATETIME        NULL,
    [Reason]            VARCHAR (500)   NULL,
    [IsRefunded]        BIT             CONSTRAINT [DF_FOB_BookingRefund_IsRefunded] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FOB_BOOKINGREFUND] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

