CREATE TABLE [dbo].[CT_MigrationFailedLeads] (
    [Id]               NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [custName]         VARCHAR (100)  NULL,
    [custMobile]       VARCHAR (10)   NULL,
    [custEmail]        VARCHAR (50)   NULL,
    [cw_lead_id]       INT            NULL,
    [cw_dealer_id]     INT            NULL,
    [model_id]         INT            NULL,
    [source]           SMALLINT       NULL,
    [lead_date]        VARCHAR (50)   NULL,
    [lead_lastUpdated] VARCHAR (50)   NULL,
    [mfgyear]          INT            NULL,
    [status]           VARCHAR (50)   NULL,
    [substatus]        VARCHAR (50)   NULL,
    [statusDate]       VARCHAR (50)   NULL,
    [custComments]     VARCHAR (1000) NULL,
    [statusCategory]   VARCHAR (50)   NULL,
    [apiResponse]      VARCHAR (2000) NULL,
    [CreatedOn]        DATETIME       CONSTRAINT [DF_CT_MigrationFailedLeads_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CT_MigrationFailedLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

