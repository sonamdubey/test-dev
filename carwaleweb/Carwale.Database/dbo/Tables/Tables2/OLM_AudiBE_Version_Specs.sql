CREATE TABLE [dbo].[OLM_AudiBE_Version_Specs] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [VersionId] INT          NOT NULL,
    [SpecId]    INT          NOT NULL,
    [Value]     VARCHAR (50) NULL,
    [IsActive]  BIT          CONSTRAINT [DF_OLM_AudiBE_Version_Specs_Active] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBEFeatures] PRIMARY KEY CLUSTERED ([Id] ASC)
);

