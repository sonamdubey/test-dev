CREATE TABLE [dbo].[OLM_CompModels] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]     NUMERIC (18) NOT NULL,
    [CompModelId] NUMERIC (18) NOT NULL,
    [UpdatedOn]   DATETIME     CONSTRAINT [DF_OLM_CompModels_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_CompModels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

