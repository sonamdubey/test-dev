CREATE TABLE [dbo].[CRM_PostData] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]             NUMERIC (18)  NULL,
    [CallId]             NUMERIC (18)  NULL,
    [TokenId]            VARCHAR (50)  NULL,
    [Status]             VARCHAR (50)  NULL,
    [Comment]            VARCHAR (500) NULL,
    [ErrorCode]          VARCHAR (20)  NULL,
    [EntryDate]          DATETIME      NULL,
    [IncomingXMLListing] VARCHAR (MAX) NULL,
    [OutgoingXMLListing] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CRM_PostData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_PostData__LeadId]
    ON [dbo].[CRM_PostData]([LeadId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0-NoStatus, 1-Accepted, 2-Rejected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PostData', @level2type = N'COLUMN', @level2name = N'Status';

