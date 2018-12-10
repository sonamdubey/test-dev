CREATE TABLE [dbo].[CRM_LeadTransferLog] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]            NUMERIC (18)  NULL,
    [TransferTeamId]    NUMERIC (18)  NULL,
    [TransferredTeamId] NUMERIC (18)  NULL,
    [TransferDate]      DATETIME      NULL,
    [TransferredBy]     NUMERIC (18)  NULL,
    [Comments]          VARCHAR (500) NULL,
    CONSTRAINT [PK_CRM_LeadTransferLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

