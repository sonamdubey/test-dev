CREATE TABLE [dbo].[CRM_SentSMS_archive_0616] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]    NUMERIC (18)  NULL,
    [SMSType]   INT           NULL,
    [EventType] INT           NULL,
    [SentBy]    NUMERIC (18)  NULL,
    [SMS]       VARCHAR (500) NULL,
    [SMSDate]   DATETIME      NULL,
    CONSTRAINT [PK_CRM_SentSMS] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_SentSMS_LeadId]
    ON [dbo].[CRM_SentSMS_archive_0616]([LeadId] ASC);

