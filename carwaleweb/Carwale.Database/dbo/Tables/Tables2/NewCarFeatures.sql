CREATE TABLE [dbo].[NewCarFeatures] (
    [CarVersionId]  NUMERIC (18) NOT NULL,
    [FeatureItemId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_NewCarFeatures_1] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [FeatureItemId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewCarFeatures_CarVersions] FOREIGN KEY ([CarVersionId]) REFERENCES [dbo].[CarVersions] ([ID]),
    CONSTRAINT [FK_NewCarFeatures_NewCarFeatureItems] FOREIGN KEY ([FeatureItemId]) REFERENCES [dbo].[NewCarFeatureItems] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [ix_NewCarFeatures_FeatureItemId]
    ON [dbo].[NewCarFeatures]([FeatureItemId] ASC);

