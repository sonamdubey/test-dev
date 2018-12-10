﻿CREATE TABLE [dbo].[TC_SpecialUsersBkp04072014] (
    [TC_SpecialUsersId]   INT                 IDENTITY (1, 1) NOT NULL,
    [UserName]            VARCHAR (100)       NOT NULL,
    [Email]               VARCHAR (100)       NOT NULL,
    [Password]            VARCHAR (20)        NOT NULL,
    [Mobile]              VARCHAR (10)        NULL,
    [Sex]                 VARCHAR (6)         NULL,
    [DOB]                 DATE                NULL,
    [Address]             VARCHAR (200)       NULL,
    [MakeId]              SMALLINT            NULL,
    [IsActive]            BIT                 NOT NULL,
    [IsFirstTimeLoggedIn] BIT                 NULL,
    [GCMRegistrationId]   VARCHAR (250)       NULL,
    [EntryDate]           DATETIME            NOT NULL,
    [ModifiedDate]        DATETIME            NULL,
    [ModifiedBy]          INT                 NULL,
    [HierId]              [sys].[hierarchyid] NULL,
    [lvl]                 SMALLINT            NULL,
    [NodeCode]            NVARCHAR (4000)     NULL,
    [ReportingType]       TINYINT             NULL,
    [Designation]         TINYINT             NULL,
    [ReportsTo]           INT                 NULL,
    [PwdRecoveryEmail]    VARCHAR (50)        NULL,
    [AliasUserId]         INT                 NULL,
    [UniqueId]            VARCHAR (50)        NULL
);

