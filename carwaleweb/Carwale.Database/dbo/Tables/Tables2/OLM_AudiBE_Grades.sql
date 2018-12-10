CREATE TABLE [dbo].[OLM_AudiBE_Grades] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_OLM_AudiBE_Grades_Active] DEFAULT ((1)) NULL,
    [Priority] INT          NULL,
    CONSTRAINT [PK_OLM_AudiBEGradeTypeMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

