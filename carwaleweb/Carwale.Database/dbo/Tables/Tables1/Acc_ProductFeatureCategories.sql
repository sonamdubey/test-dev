CREATE TABLE [dbo].[Acc_ProductFeatureCategories] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProductId] NUMERIC (18)  NOT NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_Accessories_ProductFeatureCategories_IsActive] DEFAULT ((1)) NOT NULL
);

