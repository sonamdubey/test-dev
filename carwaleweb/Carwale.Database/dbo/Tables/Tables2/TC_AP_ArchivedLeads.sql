CREATE TABLE [dbo].[TC_AP_ArchivedLeads] (
    [LeadId]      NUMERIC (18) NOT NULL,
    [UserId]      INT          NOT NULL,
    [ArchiveDate] DATETIME     CONSTRAINT [DF_TC_AP_ArchivedLeads_ArchiveDate] DEFAULT (getdate()) NULL
);

