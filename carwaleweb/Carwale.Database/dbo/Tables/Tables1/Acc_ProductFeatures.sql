CREATE TABLE [dbo].[Acc_ProductFeatures] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FeatureCategoryId] NUMERIC (18)  NOT NULL,
    [Name]              VARCHAR (100) NOT NULL,
    [ValueType]         SMALLINT      NOT NULL,
    [IsVariant]         BIT           CONSTRAINT [DF_Accessories_ProductFeatures_IsVariant] DEFAULT ((0)) NOT NULL,
    [Priority]          SMALLINT      CONSTRAINT [DF_Accessories_ProductFeatures_Priority] DEFAULT ((10)) NOT NULL,
    [IsActive]          BIT           CONSTRAINT [DF_Accessories_ProductFeatures_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Accessories_ProductFeatures] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

