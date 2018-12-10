CREATE TABLE [dbo].[Acc_ProductAdditionalFeatures] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProductId] NUMERIC (18)  NOT NULL,
    [Name]      VARCHAR (100) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_Accessories_ProductAdditionalFeatures_IsActive] DEFAULT ((1)) NOT NULL
);

