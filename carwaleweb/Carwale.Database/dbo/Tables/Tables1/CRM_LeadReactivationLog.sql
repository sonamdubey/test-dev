CREATE TABLE [dbo].[CRM_LeadReactivationLog] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]           NUMERIC (18)  NULL,
    [AssignedToTeam]   NUMERIC (18)  NULL,
    [ReactivatedBy]    NUMERIC (18)  NULL,
    [ReactivationDate] DATETIME      NULL,
    [Comments]         VARCHAR (500) NULL,
    [Reason]           VARCHAR (500) NULL,
    CONSTRAINT [PK_CRM_LeadReactivateLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

