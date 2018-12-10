CREATE TABLE [dbo].[CRM_SkodaLeadsSalesman] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TokenNo]              VARCHAR (50)  NULL,
    [AgencyId]             VARCHAR (50)  NULL,
    [AgencyLeadId]         VARCHAR (50)  NULL,
    [CompanyId]            VARCHAR (50)  NULL,
    [DMSLeadId]            VARCHAR (50)  NULL,
    [DMSLeadOpenedOn]      VARCHAR (50)  NULL,
    [TokenRecProcessOwner] VARCHAR (200) NULL,
    [SalesMan]             VARCHAR (200) NULL,
    [FromDate]             VARCHAR (50)  NULL,
    [ToDate]               VARCHAR (50)  NULL,
    [Comments]             VARCHAR (500) NULL,
    [CreatedOn]            DATETIME      CONSTRAINT [DF_CRM_SkdoaLeadsSalesman_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_SkdoaLeadsSalesman] PRIMARY KEY CLUSTERED ([Id] ASC)
);

