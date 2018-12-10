CREATE TABLE [dbo].[NewCarFeatureItems] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]       VARCHAR (200) NOT NULL,
    [CategoryId] INT           NOT NULL,
    [CarModelId] NUMERIC (18)  NOT NULL,
    [IsObsolete] BIT           CONSTRAINT [DF_NewCarFeatureItems_IsObsolete] DEFAULT (0) NULL,
    CONSTRAINT [PK_NewCarFeatureItems] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewCarFeatureItems_NewCarFeatureCategories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[NewCarFeatureCategories] ([Id])
);

