CREATE TABLE [dbo].[CRM_ADM_LogModelMapping] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [GroupType] INT          NULL,
    [ModelId]   NUMERIC (18) NULL,
    [DeletedBy] NUMERIC (18) NULL,
    [DeletedOn] DATETIME     NULL,
    CONSTRAINT [PK_CRM_ADM_LogModelMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

