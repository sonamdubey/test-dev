CREATE TABLE [dbo].[CRM_ADM_TargetType] (
    [Id]         NUMERIC (18)  NOT NULL,
    [TargetName] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRM_ADM_TargetType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

