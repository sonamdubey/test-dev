CREATE TABLE [dbo].[CarValuations_May2016] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]           NUMERIC (18)  NULL,
    [CarVersionId]         NUMERIC (18)  NOT NULL,
    [CarYear]              DATETIME      NOT NULL,
    [CustomerName]         VARCHAR (50)  NULL,
    [Email]                VARCHAR (50)  NULL,
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
    [CarCondition]         INT           NULL,
    [ActualSoldPrice]      NUMERIC (18)  CONSTRAINT [DF_CarValuations_May2016_ActualSoldPrice] DEFAULT ((0)) NULL,
    [IsActive]             BIT           CONSTRAINT [DF_CarValuations_May2016_IsActive] DEFAULT ((1)) NOT NULL,
    [IsHidden]             BIT           CONSTRAINT [DF_CarValuations_May2016_IsHidden] DEFAULT ((0)) NOT NULL,
    [RemoteHost]           VARCHAR (100) NULL,
    [RequestSource]        INT           CONSTRAINT [DF_CarValuations_May2016_RequestSource] DEFAULT ((1)) NOT NULL,
    [Kms]                  NUMERIC (18)  CONSTRAINT [DF_CarValuations_May2016_Kms] DEFAULT ((0)) NOT NULL,
    [ActualCityId]         NUMERIC (18)  NULL,
    [CityId]               NUMERIC (18)  NULL,
    CONSTRAINT [PK_CarValuationsMay2016] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Two options. 1. if the request is made from  Valuation Page, 2. If the request is made from Sell Car Page.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarValuations_May2016', @level2type = N'COLUMN', @level2name = N'RequestSource';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User actually asked valuation for this city.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarValuations_May2016', @level2type = N'COLUMN', @level2name = N'ActualCityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Valuation is provided for this city', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarValuations_May2016', @level2type = N'COLUMN', @level2name = N'CityId';

