CREATE TABLE [dbo].[OLM_AudiBE_Model_GradeFeatures] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [ModelId]   INT NOT NULL,
    [GradeId]   INT NOT NULL,
    [FeatureId] INT NOT NULL,
    [IsActive]  BIT CONSTRAINT [DF_OLM_AudiBE_Model_GradeFeatures_Active] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBEGradeValues] PRIMARY KEY CLUSTERED ([Id] ASC)
);

