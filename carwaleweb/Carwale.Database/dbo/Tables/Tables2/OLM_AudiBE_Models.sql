CREATE TABLE [dbo].[OLM_AudiBE_Models] (
    [Id]                    NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]                  VARCHAR (50) NOT NULL,
    [IsActive]              BIT          CONSTRAINT [DF_OLM_AudiBE_Models_IsActive] DEFAULT ((1)) NOT NULL,
    [MainModelColorId]      NUMERIC (18) NULL,
    [MainUpholestryColorId] NUMERIC (18) CONSTRAINT [DF_OLM_AudiBE_Models_MainUpholestryColorId] DEFAULT ((-1)) NULL,
    [CarwaleModelId]        NUMERIC (18) NULL,
    CONSTRAINT [PK_OLM_AudiBE_Models] PRIMARY KEY CLUSTERED ([Id] ASC)
);

