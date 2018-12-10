CREATE TABLE [dbo].[PQ_CategoryItemsAutoPopulateSpecification] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ItemCategoryId] INT NULL,
    [IsAutoPopulate] BIT NULL,
    [ValueType]      INT NULL,
    [Value]          INT NULL,
    [RefChargeId]    INT NULL
);

