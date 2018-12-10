﻿CREATE TABLE [dbo].[INS_PremiumLeads] (
    [ID]                  NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]          NUMERIC (18)    NULL,
    [InsTypeNew]          BIT             NOT NULL,
    [VersionId]           NUMERIC (18)    NULL,
    [MakeYear]            NVARCHAR (20)   NULL,
    [CityId]              NUMERIC (18)    NOT NULL,
    [Price]               NUMERIC (18)    NOT NULL,
    [Displacement]        NUMERIC (18)    NULL,
    [RegistrationArea]    VARCHAR (100)   NOT NULL,
    [Premium]             DECIMAL (18, 2) NULL,
    [RequestDateTime]     DATETIME        NOT NULL,
    [Name]                VARCHAR (50)    NULL,
    [Email]               VARCHAR (100)   NULL,
    [Mobile]              VARCHAR (12)    NULL,
    [PushStatus]          VARCHAR (50)    NULL,
    [ClientId]            TINYINT         NULL,
    [LeadSource]          INT             NULL,
    [CarRegistrationDate] DATETIME        NULL,
    [NoClaimBonus]        INT             NULL,
    [Quotation]           VARCHAR (800)   NULL,
    [StatusId]            TINYINT         NULL,
    [PolicyExpiryDate]    DATETIME        NULL,
    [ResponseTime]        DATETIME        NULL,
    [clientIp]            VARCHAR (30)    NULL,
    [UtmCode]             VARCHAR (20)    NULL,
    CONSTRAINT [PK_INS_PremiumLeads] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

