CREATE TABLE [dbo].[FOB_DealerBookingDetails] (
    [Id]                   NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BookingCode]          VARCHAR (50)    NULL,
    [BRId]                 NUMERIC (18)    NULL,
    [StockId]              NUMERIC (18)    NULL,
    [DealerId]             NUMERIC (18)    NULL,
    [Amount]               NUMERIC (18, 2) NULL,
    [PaymentMode]          SMALLINT        NULL,
    [EntryDateTime]        DATETIME        NULL,
    [PaymentDetails]       VARCHAR (150)   NULL,
    [DealerOrganizationID] NUMERIC (18)    NULL,
    CONSTRAINT [PK_FOB_DealerBookingDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

