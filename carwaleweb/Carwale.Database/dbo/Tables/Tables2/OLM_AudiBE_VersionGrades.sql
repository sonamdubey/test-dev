CREATE TABLE [dbo].[OLM_AudiBE_VersionGrades] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId] NUMERIC (18) NULL,
    [GradeId]   NUMERIC (18) NULL,
    [IsActive]  BIT          CONSTRAINT [DF_OLM_AudiBE_VersionGrades_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBE_VersionGrades] PRIMARY KEY CLUSTERED ([Id] ASC)
);

