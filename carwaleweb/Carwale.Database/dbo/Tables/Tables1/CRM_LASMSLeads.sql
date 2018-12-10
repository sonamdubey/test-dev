CREATE TABLE [dbo].[CRM_LASMSLeads] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FirstName]          VARCHAR (100) NULL,
    [LastName]           VARCHAR (100) NULL,
    [EMail]              VARCHAR (100) NULL,
    [Mobile]             VARCHAR (50)  NULL,
    [Landline]           VARCHAR (50)  NULL,
    [CityId]             NUMERIC (18)  NULL,
    [VersionId]          NUMERIC (18)  NULL,
    [ExpectedBuyingDate] DATETIME      NULL,
    [LAID]               NUMERIC (18)  NOT NULL,
    [Status]             SMALLINT      NULL,
    [Category]           VARCHAR (100) NULL,
    [LeadSource]         VARCHAR (200) NULL,
    [CRMLeadId]          NUMERIC (18)  NULL,
    [EntryDateTime]      DATETIME      NOT NULL,
    CONSTRAINT [PK_CRM_LASMSLeads] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

