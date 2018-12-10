CREATE TABLE [dbo].[CRM_NCDDumpedLeads] (
    [CallId]      NUMERIC (18) NOT NULL,
    [DisposeDate] DATETIME     CONSTRAINT [DF_CRM_NCDDumpedLeads_DisposeDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_NCDDumpedLeads] PRIMARY KEY CLUSTERED ([CallId] ASC)
);

