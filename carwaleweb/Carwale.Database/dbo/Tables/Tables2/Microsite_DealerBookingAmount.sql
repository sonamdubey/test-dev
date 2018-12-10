CREATE TABLE [dbo].[Microsite_DealerBookingAmount] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]      NUMERIC (18) NOT NULL,
    [UpperLimit]    INT          NULL,
    [BookingAmount] VARCHAR (25) NOT NULL,
    [IsActive]      BIT          NOT NULL,
    CONSTRAINT [PK_Microsite_DealerBookingAmount] PRIMARY KEY CLUSTERED ([Id] ASC)
);

