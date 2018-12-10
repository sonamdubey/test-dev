CREATE TABLE [dbo].[OLM_SkodaFeatureGroup] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [ModelId]  INT          NOT NULL,
    [IsActive] BIT          NOT NULL,
    CONSTRAINT [PK_OLM_SkodaFeatureGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

