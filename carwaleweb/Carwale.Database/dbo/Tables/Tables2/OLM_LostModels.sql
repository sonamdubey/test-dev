CREATE TABLE [dbo].[OLM_LostModels] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ModelId]     NUMERIC (18)  NOT NULL,
    [LostModelId] NUMERIC (18)  NOT NULL,
    [MonthVal]    DATETIME      NOT NULL,
    [Reason]      VARCHAR (200) NOT NULL,
    [TCount]      NUMERIC (18)  NOT NULL,
    [LastUpdated] DATETIME      CONSTRAINT [DF_OLM_LostModels_LastUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_LostModels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

