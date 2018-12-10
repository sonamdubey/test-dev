CREATE TABLE [dbo].[PQ_Category] (
    [CategoryId]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CategoryName]         VARCHAR (50) NULL,
    [SortOrder]            TINYINT      NOT NULL,
    [IsMultiplePriceExist] BIT          CONSTRAINT [DF_PQ_Category_IsMultiplePriceExist] DEFAULT ((0)) NULL
);

