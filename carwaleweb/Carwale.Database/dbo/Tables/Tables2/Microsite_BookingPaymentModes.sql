CREATE TABLE [dbo].[Microsite_BookingPaymentModes] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [PaymentMode] VARCHAR (100) NULL,
    CONSTRAINT [PK_Microsite_BookingPaymentModes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

