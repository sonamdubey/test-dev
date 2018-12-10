CREATE TABLE [dbo].[Failed_PQDealerAdLeads] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [PQId]             NUMERIC (18)  NOT NULL,
    [CampaignId]       INT           NOT NULL,
    [LeadClickSource]  SMALLINT      NULL,
    [RequestDateTime]  DATETIME      NULL,
    [Name]             VARCHAR (100) NULL,
    [Email]            VARCHAR (100) NULL,
    [Mobile]           VARCHAR (100) NULL,
    [AssignedDealerID] INT           NULL,
    [CityId]           INT           NULL,
    [ZoneId]           INT           NULL,
    [VersionId]        INT           NULL,
    [PlatformId]       INT           NULL,
    [LTSRC]            VARCHAR (50)  NULL,
    [Comment]          VARCHAR (500) NULL,
    [ErrorMsg]         VARCHAR (MAX) NULL,
    [ModelId]          INT           NULL
);

