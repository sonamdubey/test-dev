CREATE TABLE [dbo].[Acc_ItemsFeatures] (
    [Id]           NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemId]       NUMERIC (18)    NOT NULL,
    [FeatureId]    NUMERIC (18)    NOT NULL,
    [BooleanValue] BIT             NULL,
    [NumericValue] NUMERIC (18)    NULL,
    [DecimalValue] DECIMAL (18, 2) NULL,
    [TextValue]    VARCHAR (100)   NULL,
    [ValueType]    SMALLINT        NOT NULL,
    [IsActive]     BIT             CONSTRAINT [DF_Accessories_ItemsFeatures_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Accessories_ItemsFeatures] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_ItemsFeatures__ItemId__IsActive]
    ON [dbo].[Acc_ItemsFeatures]([ItemId] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_ItemsFeatures__FeatureId]
    ON [dbo].[Acc_ItemsFeatures]([FeatureId] ASC)
    INCLUDE([ItemId], [NumericValue]);

