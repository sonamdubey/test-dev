CREATE TABLE [dbo].[FOB_CWBookingDetails] (
    [Id]             NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BookingCode]    VARCHAR (50)    NULL,
    [BRId]           NUMERIC (18)    NULL,
    [StockId]        NUMERIC (18)    NULL,
    [DealerId]       NUMERIC (18)    NULL,
    [Amount]         NUMERIC (18, 2) NULL,
    [PaymentMode]    SMALLINT        NULL,
    [EntryDateTime]  DATETIME        NULL,
    [PaymentDetails] VARCHAR (150)   NULL,
    CONSTRAINT [PK_FOB_BookingDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

