CREATE TABLE [dbo].[OLM_AudiBE_Features] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (150) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_OLM_AudiBE_Features_Active] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OLM_AudiBEGradeMaster] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OLM_AudiBE_Features_OLM_AudiBE_Features] FOREIGN KEY ([Id]) REFERENCES [dbo].[OLM_AudiBE_Features] ([Id])
);

