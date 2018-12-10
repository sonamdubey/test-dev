CREATE TABLE [dbo].[CRM_CallReopenLog] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CallId]     NUMERIC (18)  NULL,
    [CallType]   INT           NULL,
    [LeadId]     NUMERIC (18)  NULL,
    [TeamId]     NUMERIC (18)  NULL,
    [ReopenedBy] NUMERIC (18)  NULL,
    [Subject]    VARCHAR (500) NULL,
    [ReopenDate] DATETIME      NOT NULL,
    CONSTRAINT [PK_CRM_ReopenCallLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

