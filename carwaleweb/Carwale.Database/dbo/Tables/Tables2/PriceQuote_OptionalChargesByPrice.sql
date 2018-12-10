CREATE TABLE [dbo].[PriceQuote_OptionalChargesByPrice] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId]     INT NOT NULL,
    [OptionalCategoryId] INT NOT NULL,
    [MakeId]             INT NOT NULL
);

