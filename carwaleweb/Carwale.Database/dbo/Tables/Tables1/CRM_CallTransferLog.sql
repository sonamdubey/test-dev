CREATE TABLE [dbo].[CRM_CallTransferLog] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CallId]        NUMERIC (18) NULL,
    [LeadId]        NUMERIC (18) NULL,
    [FromCaller]    NUMERIC (18) NULL,
    [ToCaller]      NUMERIC (18) NULL,
    [TransferDate]  DATETIME     NULL,
    [TransferredBy] NUMERIC (18) NULL,
    [TransferType]  SMALLINT     NULL,
    CONSTRAINT [PK_CRM_CallTransferLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallTransferLog]
    ON [dbo].[CRM_CallTransferLog]([TransferType] ASC)
    INCLUDE([LeadId], [TransferDate]);


GO
CREATE NONCLUSTERED INDEX [IX_TransferType]
    ON [dbo].[CRM_CallTransferLog]([ToCaller] ASC, [TransferType] ASC)
    INCLUDE([LeadId], [TransferDate]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallTransferLog_LeadId]
    ON [dbo].[CRM_CallTransferLog]([LeadId] ASC)
    INCLUDE([FromCaller], [ToCaller], [TransferDate], [TransferredBy], [TransferType]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-VerificationCall, 2-ConsultationCall', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_CallTransferLog', @level2type = N'COLUMN', @level2name = N'TransferType';

