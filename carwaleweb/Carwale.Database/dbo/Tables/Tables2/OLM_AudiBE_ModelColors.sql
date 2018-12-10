CREATE TABLE [dbo].[OLM_AudiBE_ModelColors] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]     NUMERIC (18) NULL,
    [ColorId]     NUMERIC (18) NULL,
    [ColorForId]  NUMERIC (18) NULL,
    [ColorTypeId] NUMERIC (18) NULL,
    [IsActive]    BIT          CONSTRAINT [DF_OLM_AudiBE_ModelColors_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBE_ModelColors] PRIMARY KEY CLUSTERED ([Id] ASC)
);

