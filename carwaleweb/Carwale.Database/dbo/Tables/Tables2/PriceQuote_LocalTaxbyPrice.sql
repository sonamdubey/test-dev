CREATE TABLE [dbo].[PriceQuote_LocalTaxbyPrice] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [CityId]           INT          NOT NULL,
    [Pricediff_Lower]  NUMERIC (18) NULL,
    [Pricediff_Higher] NUMERIC (18) NULL,
    [Rate]             FLOAT (53)   NOT NULL
);

