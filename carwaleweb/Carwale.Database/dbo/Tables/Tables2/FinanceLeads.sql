﻿CREATE TABLE [dbo].[FinanceLeads] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]           NUMERIC (18)  CONSTRAINT [DF_FinanceLeads_CustomerId] DEFAULT ((-1)) NOT NULL,
    [VersionId]            NUMERIC (18)  NOT NULL,
    [UsedCarProfileId]     VARCHAR (50)  NOT NULL,
    [isUsed]               BIT           CONSTRAINT [DF_FinanceLeads_isUsed] DEFAULT ((1)) NOT NULL,
    [Tenure]               INT           NOT NULL,
    [FinanceAmountPercent] INT           NOT NULL,
    [FirstName]            VARCHAR (50)  NOT NULL,
    [LastName]             VARCHAR (50)  NOT NULL,
    [Email]                VARCHAR (100) NOT NULL,
    [Phone1]               VARCHAR (50)  NULL,
    [Mobile]               VARCHAR (50)  NULL,
    [CityId]               NUMERIC (18)  NOT NULL,
    [Comments]             VARCHAR (500) NULL,
    [EntryDate]            DATETIME      NOT NULL,
    [IsApproved]           BIT           CONSTRAINT [DF_FinanceLeads_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsFake]               BIT           CONSTRAINT [DF_FinanceLeads_IsFake] DEFAULT ((0)) NOT NULL,
    [IsForwarded]          BIT           CONSTRAINT [DF_FinanceLeads_IsForwarded] DEFAULT ((0)) NOT NULL,
    [IsRejected]           BIT           CONSTRAINT [DF_FinanceLeads_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsMailSend]           BIT           CONSTRAINT [DF_FinanceLeads_IsMailSend] DEFAULT ((0)) NOT NULL,
    [IsViewed]             BIT           CONSTRAINT [DF_FinanceLeads_IsViewed] DEFAULT ((0)) NOT NULL,
    [ContactNo]            VARCHAR (50)  NULL,
    [Finalized]            VARCHAR (50)  NULL,
    [Age]                  VARCHAR (100) NULL,
    [SourceId]             SMALLINT      CONSTRAINT [DF_FinanceLeads_SourceId] DEFAULT ((1)) NOT NULL,
    [LoanAmount]           NUMERIC (18)  NULL,
    [MonthlyIncome]        NUMERIC (18)  NULL,
    [Dob]                  DATETIME      NULL,
    [EmpType]              VARCHAR (50)  NULL,
    CONSTRAINT [PK_FinanceLeads] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

