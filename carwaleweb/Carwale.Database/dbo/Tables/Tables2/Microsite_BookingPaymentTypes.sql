CREATE TABLE [dbo].[Microsite_BookingPaymentTypes] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [PaymentType] VARCHAR (100) NULL,
    CONSTRAINT [PK_Microsite_BookingPaymentTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

