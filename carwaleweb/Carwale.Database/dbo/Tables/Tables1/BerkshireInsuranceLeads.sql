CREATE TABLE [dbo].[BerkshireInsuranceLeads] (
    [BerkshireLeadId]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [CarwaleCustomerId]       BIGINT        NOT NULL,
    [BerkshireCityId]         SMALLINT      NOT NULL,
    [BerkshireMakeId]         SMALLINT      NOT NULL,
    [BerkshireModelId]        SMALLINT      NOT NULL,
    [BerkshireVersionId]      SMALLINT      NOT NULL,
    [CarMakeYear]             SMALLINT      NULL,
    [CarRegistrationDate]     DATE          NULL,
    [PolicyType]              VARCHAR (20)  NULL,
    [CurrentPolicyExpiryDate] DATE          NULL,
    [IsPushedToBerkshire]     BIT           CONSTRAINT [DF_BerkshireInsuranceLeads_IsPushedToBerkshire] DEFAULT ((0)) NOT NULL,
    [JsonRequestString]       VARCHAR (MAX) NULL,
    [EntryDate]               DATETIME      NOT NULL,
    [ReturnedBerkshireLeadId] BIGINT        NULL,
    [PushStatusMessage]       VARCHAR (200) NULL,
    [CustomerName]            VARCHAR (100) NULL,
    [CustomerEmail]           VARCHAR (50)  NULL,
    [CustomerMobile]          VARCHAR (12)  NULL,
    [ResponseEntryDate]       DATETIME      NULL
);

