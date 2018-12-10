CREATE TABLE [dbo].[CRM_CallCheckLog] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CallerId]      VARCHAR (50) NULL,
    [EntryDateTime] DATETIME     CONSTRAINT [DF_CRM_CallCheckLog_EntryDateTime] DEFAULT (getdate()) NULL,
    [CallId]        VARCHAR (50) NULL,
    [CallEntryDate] DATETIME     NULL,
    CONSTRAINT [PK_CRM_CallCheckLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

