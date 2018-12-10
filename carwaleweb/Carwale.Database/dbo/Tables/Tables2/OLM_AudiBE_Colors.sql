CREATE TABLE [dbo].[OLM_AudiBE_Colors] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (150) NOT NULL,
    [Code]     VARCHAR (10)  NULL,
    [HashCode] VARCHAR (10)  NULL,
    [IsActive] BIT           CONSTRAINT [DF_OLM_AudiBE_Colors_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_OLM_AudiBE_Colors] PRIMARY KEY CLUSTERED ([Id] ASC)
);

