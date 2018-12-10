CREATE TABLE [dbo].[OLM_AudiBE_VersionPrices] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [VersionId] NUMERIC (18) NULL,
    [GradeId]   NUMERIC (18) NULL,
    [StateId]   NUMERIC (18) NULL,
    [Price]     NUMERIC (18) NULL,
    [IsActive]  BIT          CONSTRAINT [DF_OLM_AudiBE_VersionPrices_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBE_VersionPrices] PRIMARY KEY CLUSTERED ([Id] ASC)
);

