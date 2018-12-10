CREATE TABLE [dbo].[PQ_CategoryItems_log] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT           NOT NULL,
    [CategoryId]     TINYINT       NOT NULL,
    [CategoryName]   VARCHAR (200) NOT NULL,
    [Type]           INT           NULL,
    [Scope]          INT           NULL,
    [UpdatedBy]      INT           NULL,
    [UpdatedOn]      DATETIME      NULL,
    [IsActive]       BIT           NULL,
    [Changes]        VARCHAR (MAX) NULL,
    [LogMessage]     VARCHAR (MAX) NULL
);

