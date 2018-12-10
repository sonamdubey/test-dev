CREATE TABLE [dbo].[Acc_ItemsAdditionalFeatures] (
    [Id]                   NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemId]               NUMERIC (18) NOT NULL,
    [AdditionalFeaturesId] NUMERIC (18) NOT NULL,
    [IsActive]             BIT          CONSTRAINT [DF_Accessories_ItemsAdditionalFeatures_IsActice] DEFAULT ((1)) NOT NULL
);

