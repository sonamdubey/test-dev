CREATE TABLE [dbo].[OLM_AudiBE_Specs] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_OLM_AudiBE_Specs_Active] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBEFeaturesMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

