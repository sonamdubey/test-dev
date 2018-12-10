﻿CREATE TABLE [dbo].[AxisBank_CarValuations] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CustomerId]           NUMERIC (18)  NULL,
    [FileReferenceNumber]  VARCHAR (20)  NULL,
    [RegistrationNumber]   VARCHAR (50)  NULL,
    [CarVersionId]         NUMERIC (18)  NOT NULL,
    [CarYear]              DATETIME      NOT NULL,
    [City]                 VARCHAR (50)  NULL,
    [RequestDateTime]      DATETIME      NOT NULL,
    [ValueExcellent]       NUMERIC (18)  NULL,
    [ValueGood]            NUMERIC (18)  NULL,
    [ValueFair]            NUMERIC (18)  NULL,
    [ValuePoor]            NUMERIC (18)  NULL,
    [ValueExcellentDealer] NUMERIC (18)  NULL,
    [ValueGoodDealer]      NUMERIC (18)  NULL,
    [ValueFairDealer]      NUMERIC (18)  NULL,
    [ValuePoorDealer]      NUMERIC (18)  NULL,
    [CarCondition]         VARCHAR (20)  NULL,
    [ActualSoldPrice]      NUMERIC (18)  CONSTRAINT [DF_AxisBank_CarValuations_ActualSoldPrice] DEFAULT ((0)) NULL,
    [IsActive]             BIT           CONSTRAINT [DF_AxisBank_CarValuations_IsActive] DEFAULT ((1)) NOT NULL,
    [RemoteHost]           VARCHAR (100) NULL,
    [Kms]                  NUMERIC (18)  CONSTRAINT [DF_AxisBank_CarValuations_Kms] DEFAULT ((0)) NOT NULL,
    [ActualCityId]         NUMERIC (18)  NULL,
    [CityId]               NUMERIC (18)  NULL,
    [RequestSource]        INT           CONSTRAINT [DF_AxisBank_CarValuations_RequestSource] DEFAULT ((1)) NULL,
    [ASC_Id]               NUMERIC (18)  NULL,
    CONSTRAINT [PK_AxisBank_CarValuations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

