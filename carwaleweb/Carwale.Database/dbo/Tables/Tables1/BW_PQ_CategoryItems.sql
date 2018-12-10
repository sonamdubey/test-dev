CREATE TABLE [dbo].[BW_PQ_CategoryItems] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT           NULL,
    [ItemName]       VARCHAR (100) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_BW_PQ_CategoryItems_IsActive] DEFAULT ((1)) NULL
);

