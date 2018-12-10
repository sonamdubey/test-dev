CREATE TABLE [dbo].[ICB_StampDuty] (
    [Id]              NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FAID]            NUMERIC (18)    NULL,
    [CityId]          NUMERIC (18)    NULL,
    [Rate]            NUMERIC (6, 3)  NULL,
    [FixedFee]        NUMERIC (18)    NULL,
    [ServiceTax]      NUMERIC (5, 2)  NULL,
    [StartPriceRange] DECIMAL (18, 2) NULL,
    [EndPriceRange]   DECIMAL (18, 2) NULL,
    [LastUpdated]     DATETIME        NULL,
    CONSTRAINT [PK_ICB_StampDuty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

