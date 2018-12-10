CREATE TABLE [dbo].[CRM_ConsultantApproval] (
    [CCA_Id]     NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]     NUMERIC (18) NOT NULL,
    [EntryDate]  DATETIME     CONSTRAINT [DF_CRM_ConsultantApproval_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsApproved] BIT          NULL,
    [UpdatedOn]  DATETIME     NULL,
    CONSTRAINT [PK_CRM_ConsultantApproval] PRIMARY KEY CLUSTERED ([CCA_Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_ConsultantApproval_LeadId]
    ON [dbo].[CRM_ConsultantApproval]([LeadId] ASC);

