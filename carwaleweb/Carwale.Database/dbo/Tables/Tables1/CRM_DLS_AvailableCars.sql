CREATE TABLE [dbo].[CRM_DLS_AvailableCars] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [AvaliableModelId] INT          NOT NULL,
    CONSTRAINT [PK_CRM_DLS_AvailableCars] PRIMARY KEY CLUSTERED ([Id] ASC)
);

