CREATE TABLE [dbo].[OLM_AudiBE_ColorTypes] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_OLM_AudiBE_ColorTypes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_OLM_AudiBE_ColorTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

