CREATE TABLE [dbo].[CRM_ADM_AutoVerifyRules] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MakeId]    NUMERIC (18) NOT NULL,
    [AliasName] VARCHAR (50) NULL,
    [SourceId]  INT          NOT NULL,
    [QueueId]   INT          NOT NULL,
    CONSTRAINT [PK_CRM_ADM_AutoVerifyRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

