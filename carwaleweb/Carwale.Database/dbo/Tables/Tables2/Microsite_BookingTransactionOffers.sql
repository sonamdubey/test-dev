CREATE TABLE [dbo].[Microsite_BookingTransactionOffers] (
    [Id]                             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Microsite_BookingTransactionId] NUMERIC (18) NULL,
    [Microsite_DealerOffersId]       INT          NULL,
    [EntryDate]                      DATETIME     CONSTRAINT [DF_Microsite_BookingTransactionOffers_EntryDate] DEFAULT (getdate()) NULL
);

