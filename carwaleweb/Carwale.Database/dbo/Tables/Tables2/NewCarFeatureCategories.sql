CREATE TABLE [dbo].[NewCarFeatureCategories] (
    [Id]       INT          NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_NewCarSpecificationCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_NewCarSpecificationCategories] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

