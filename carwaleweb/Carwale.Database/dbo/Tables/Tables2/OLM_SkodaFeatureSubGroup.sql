CREATE TABLE [dbo].[OLM_SkodaFeatureSubGroup] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100) NULL,
    [GroupId]  INT           NOT NULL,
    [ModelId]  INT           NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_OLM_SkodaFeatureSubGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

