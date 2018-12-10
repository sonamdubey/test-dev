CREATE TABLE [dbo].[PriceQuote2011] (
    [CWCustomerID]    NUMERIC (18) NOT NULL,
    [PQID]            NUMERIC (18) NOT NULL,
    [BuyTime]         VARCHAR (50) NULL,
    [Make]            VARCHAR (30) NOT NULL,
    [Model]           VARCHAR (30) NOT NULL,
    [Version]         VARCHAR (50) NULL,
    [RTOPrice]        NUMERIC (18) NOT NULL,
    [ExShowroomPrice] NUMERIC (18) NOT NULL,
    [InsurancePrice]  NUMERIC (18) NOT NULL,
    [BookingDate]     DATETIME     NULL,
    [CarBooked]       BIT          NULL
);

