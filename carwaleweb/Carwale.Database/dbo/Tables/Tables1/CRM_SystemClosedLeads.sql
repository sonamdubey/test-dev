CREATE TABLE [dbo].[CRM_SystemClosedLeads] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LeadId]   NUMERIC (18)  NOT NULL,
    [ClosedOn] DATETIME      CONSTRAINT [DF_CRM_SystemClosedLeads_ClosedOn] DEFAULT (getdate()) NOT NULL,
    [ClosedBy] INT           NOT NULL,
    [Reason]   VARCHAR (250) NULL,
    CONSTRAINT [PK_CRM_SystemClosedLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

