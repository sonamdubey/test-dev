CREATE TABLE [dbo].[OLM_AudiBE_Model_GradeFeatures_VersionSpecifics] (
    [ID]                  INT IDENTITY (1, 1) NOT NULL,
    [ModelGradeFeatureId] INT NOT NULL,
    [VersionId]           INT NULL,
    [IsActive]            BIT NULL
);

