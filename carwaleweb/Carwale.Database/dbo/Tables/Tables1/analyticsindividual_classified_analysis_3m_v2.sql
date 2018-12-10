﻿CREATE TABLE [dbo].[analyticsindividual_classified_analysis_3m_v2] (
    [ID]                   NUMERIC (18)  NOT NULL,
    [CustomerId]           NUMERIC (18)  NOT NULL,
    [CityId]               INT           NULL,
    [CarVersionId]         NUMERIC (18)  NOT NULL,
    [CarRegNo]             VARCHAR (50)  NOT NULL,
    [EntryDate]            DATETIME      NOT NULL,
    [Price]                DECIMAL (18)  NOT NULL,
    [MakeYear]             DATETIME      NOT NULL,
    [Kilometers]           NUMERIC (18)  NOT NULL,
    [Color]                VARCHAR (100) NULL,
    [ColorCode]            VARCHAR (6)   NULL,
    [Comments]             VARCHAR (500) NULL,
    [IsArchived]           BIT           NOT NULL,
    [IsApproved]           BIT           NOT NULL,
    [ForwardDealers]       BIT           NOT NULL,
    [ListInClassifieds]    BIT           NOT NULL,
    [IsFake]               BIT           NOT NULL,
    [StatusId]             SMALLINT      NOT NULL,
    [LastBidDate]          DATETIME      NULL,
    [ClassifiedExpiryDate] DATETIME      NULL,
    [ViewCount]            NUMERIC (18)  NULL,
    [PaidInqLeft]          INT           NOT NULL,
    [FreeInqLeft]          INT           NOT NULL,
    [PackageType]          SMALLINT      NOT NULL,
    [PackageExpiryDate]    DATETIME      NULL,
    [SourceId]             SMALLINT      NOT NULL,
    [IsVerified]           BIT           NULL,
    [CarRegState]          CHAR (4)      NULL,
    [PinCode]              INT           NULL,
    [CallType]             SMALLINT      NOT NULL,
    [TBCType]              SMALLINT      NOT NULL,
    [TBCID]                NUMERIC (18)  NOT NULL,
    [EventId]              NUMERIC (18)  NOT NULL,
    [EntryDateTime]        DATETIME      NOT NULL,
    [callid]               NUMERIC (18)  NOT NULL,
    [tcid]                 NUMERIC (18)  NOT NULL,
    [scheduleddatetime]    DATETIME      NOT NULL,
    [calleddatetime]       DATETIME      NOT NULL,
    [actionid]             NUMERIC (18)  NOT NULL,
    [agent_comment]        VARCHAR (500) NOT NULL,
    [LoginId]              VARCHAR (50)  NOT NULL,
    [Name]                 VARCHAR (50)  NOT NULL
);

