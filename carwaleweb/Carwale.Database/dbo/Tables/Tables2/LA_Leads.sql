CREATE TABLE [dbo].[LA_Leads] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FirstName]          VARCHAR (100) NULL,
    [LastName]           VARCHAR (100) NULL,
    [Email]              VARCHAR (100) NULL,
    [Mobile]             VARCHAR (50)  NULL,
    [Landline]           VARCHAR (50)  NULL,
    [CityId]             NUMERIC (18)  NULL,
    [CityName]           VARCHAR (50)  NULL,
    [VersionId]          NUMERIC (18)  NULL,
    [CarName]            VARCHAR (150) NULL,
    [ExpectedBuyingDate] DATETIME      NULL,
    [LAId]               NUMERIC (18)  NOT NULL,
    [ReferenceId]        VARCHAR (50)  NULL,
    [EntryDateTime]      DATETIME      NOT NULL,
    [CRMLeadId]          NUMERIC (18)  NULL,
    [Status]             SMALLINT      NULL,
    [IsTesting]          BIT           CONSTRAINT [DF_LA_Leads_IsTesting] DEFAULT ((1)) NOT NULL,
    [MakeId]             NUMERIC (18)  CONSTRAINT [DF_LA_Leads_MakeId] DEFAULT ((-1)) NOT NULL,
    [LeadSource]         VARCHAR (100) NULL,
    [utm_source]         VARCHAR (100) NULL,
    [utm_medium]         VARCHAR (100) NULL,
    [utm_content]        VARCHAR (100) NULL,
    [utm_campaign]       VARCHAR (100) NULL,
    [PageName]           VARCHAR (50)  NULL,
    CONSTRAINT [PK_LA_Leads] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRMLeadId]
    ON [dbo].[LA_Leads]([CRMLeadId] ASC);

