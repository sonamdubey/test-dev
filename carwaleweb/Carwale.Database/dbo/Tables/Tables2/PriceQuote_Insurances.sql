CREATE TABLE [dbo].[PriceQuote_Insurances] (
    [Id]             SMALLINT IDENTITY (1, 1) NOT NULL,
    [MakeId]         INT      NULL,
    [CategoryItemId] INT      NOT NULL,
    [IsActive]       BIT      NULL,
    [IsGeneric]      BIT      NULL
);

