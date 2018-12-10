CREATE TABLE [dbo].[OLM_AudiBE_Versions] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (150) NOT NULL,
    [ModelId]  NUMERIC (18)  NULL,
    [IsActive] BIT           NULL,
    CONSTRAINT [PK_OLM_AudiBE_Versions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

