CREATE TABLE [dbo].[PQDealerAdLeads] (
    [Id]                     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [PQId]                   NUMERIC (18)  NOT NULL,
    [DealerId]               INT           NOT NULL,
    [PushStatus]             INT           NULL,
    [LeadClickSource]        SMALLINT      NULL,
    [DealerLeadBusinessType] SMALLINT      NULL,
    [RequestDateTime]        DATETIME      DEFAULT (getdate()) NULL,
    [Name]                   VARCHAR (100) NULL,
    [Email]                  VARCHAR (100) NULL,
    [Mobile]                 VARCHAR (100) NULL,
    [AssignedDealerID]       INT           NULL,
    [CityId]                 INT           NULL,
    [ZoneId]                 INT           NULL,
    [VersionId]              INT           NULL,
    [PlatformId]             INT           NULL,
    [LTSRC]                  VARCHAR (50)  NULL,
    [Comment]                VARCHAR (500) NULL,
    [UtmaCookie]             VARCHAR (500) NULL,
    [UtmzCookie]             VARCHAR (500) NULL,
    [CampaignId]             NUMERIC (18)  NULL,
    [ModelHistory]           VARCHAR (200) NULL,
    [CWCookieValue]          VARCHAR (100) NULL,
    [ClientIP]               VARCHAR (50)  NULL,
    [Browser]                VARCHAR (100) NULL,
    [OperatingSystem]        VARCHAR (50)  NULL,
    [ABTest]                 SMALLINT      NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_PQDealerAdLeads]
    ON [dbo].[PQDealerAdLeads]([Id] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_pqdealeradleads_PQId]
    ON [dbo].[PQDealerAdLeads]([PQId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQDealerAdLeads_PushStatus]
    ON [dbo].[PQDealerAdLeads]([PushStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQDealerAdLeads_DealerId]
    ON [dbo].[PQDealerAdLeads]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQDealerAdLeads_Mobile_RequestDateTime]
    ON [dbo].[PQDealerAdLeads]([Mobile] ASC, [RequestDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PQDealerAdLeads_VersionId]
    ON [dbo].[PQDealerAdLeads]([VersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_PQDealerAdLeads_CWCookieValue]
    ON [dbo].[PQDealerAdLeads]([CWCookieValue] ASC, [RequestDateTime] ASC);

