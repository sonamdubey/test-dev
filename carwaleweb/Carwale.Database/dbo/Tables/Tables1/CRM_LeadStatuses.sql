CREATE TABLE [dbo].[CRM_LeadStatuses] (
    [CRM_LeadStatus_Id] SMALLINT      NOT NULL,
    [Name]              VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CRM_LeadStatus] PRIMARY KEY CLUSTERED ([CRM_LeadStatus_Id] ASC) WITH (FILLFACTOR = 90)
);

