CREATE TABLE [dbo].[PriceQuote_DealerCharges] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT NOT NULL,
    [IsActive]       BIT NOT NULL
);

