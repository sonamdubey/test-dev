CREATE TABLE [dbo].[CarTradeNewCarLeads] (
    [CarTradeNewCarLeadsId] NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ModelId]               INT           NULL,
    [VersionId]             INT           NULL,
    [CityId]                INT           NULL,
    [PlatformSourceId]      INT           NULL,
    [PartnerSourceId]       VARCHAR (20)  NULL,
    [Name]                  VARCHAR (100) NULL,
    [Mobile]                VARCHAR (15)  NULL,
    [Email]                 VARCHAR (150) NULL,
    [RequestDateTime]       DATETIME      NULL,
    [PqDealerLeadId]        INT           NULL
);

