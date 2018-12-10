CREATE TABLE [dbo].[OLM_SkodaFeatures] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (300) NOT NULL,
    [SubGroupId] INT           NOT NULL,
    [ModelId]    INT           NOT NULL,
    [IsActive]   BIT           NOT NULL,
    CONSTRAINT [PK_OLM_SkodaFeatures] PRIMARY KEY CLUSTERED ([Id] ASC)
);

