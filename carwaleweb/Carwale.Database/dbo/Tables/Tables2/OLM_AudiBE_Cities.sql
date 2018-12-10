CREATE TABLE [dbo].[OLM_AudiBE_Cities] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [StateId]  NUMERIC (18) NULL,
    [IsActive] BIT          CONSTRAINT [DF_OLM_AudiBE_Cities_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBE_Cities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

