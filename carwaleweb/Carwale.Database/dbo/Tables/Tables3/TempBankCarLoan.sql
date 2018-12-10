﻿CREATE TABLE [dbo].[TempBankCarLoan] (
    [Id]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FirstName]          VARCHAR (100)  NULL,
    [MiddleName]         VARCHAR (100)  NULL,
    [LastName]           VARCHAR (100)  NULL,
    [DOB]                DATETIME       NULL,
    [Sex]                VARCHAR (50)   NULL,
    [MaritialStatus]     VARCHAR (50)   NULL,
    [Qualification]      VARCHAR (50)   NULL,
    [ResPhoneNo]         VARCHAR (50)   NULL,
    [MobileNo]           VARCHAR (50)   NULL,
    [ResAddress]         VARCHAR (1000) NULL,
    [ResCityId]          NUMERIC (18)   NULL,
    [ResPincode]         VARCHAR (50)   NULL,
    [ResStatus]          VARCHAR (50)   NULL,
    [ResYear]            VARCHAR (50)   NULL,
    [ResMonth]           VARCHAR (50)   NULL,
    [EMail]              VARCHAR (100)  NULL,
    [IdType]             VARCHAR (50)   NULL,
    [OtherIdName]        VARCHAR (50)   NULL,
    [IdNumber]           VARCHAR (50)   NULL,
    [NoOfDependents]     VARCHAR (50)   NULL,
    [GrossMonthlyIncome] NUMERIC (18)   NULL,
    [OccupationType]     VARCHAR (50)   NULL,
    [CompanyName]        VARCHAR (100)  NULL,
    [CompanyType]        VARCHAR (50)   NULL,
    [BusinessNature]     VARCHAR (50)   NULL,
    [Designation]        VARCHAR (50)   NULL,
    [BusinessYear]       VARCHAR (50)   NULL,
    [BusinessMonth]      VARCHAR (50)   NULL,
    [OfficeAddress]      VARCHAR (1000) NULL,
    [OfficeCityId]       NUMERIC (18)   NULL,
    [OfficePinCode]      VARCHAR (50)   NULL,
    [OfficePhoneNo]      VARCHAR (50)   NULL,
    [DelCity]            NUMERIC (18)   NULL,
    [CarVersion]         NUMERIC (18)   NULL,
    [RegType]            VARCHAR (50)   NULL,
    [CarPrice]           NUMERIC (18)   NULL,
    [LoanAmount]         NUMERIC (18)   NULL,
    [TenureInMonths]     NUMERIC (18)   NULL,
    [EntryDate]          DATETIME       NULL,
    [IsCompleted]        BIT            CONSTRAINT [DF_TempBankCarLoan_IsCompleted] DEFAULT ((0)) NULL,
    [ReferenceNo]        VARCHAR (50)   NULL,
    [IsActive]           BIT            CONSTRAINT [DF_TempBankCarLoan_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ICICIBankCarLoan] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

