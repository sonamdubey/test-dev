CREATE TABLE [dbo].[NewCarPrices] (
    [ID]                NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId]      NUMERIC (18)   NOT NULL,
    [DealerId]          NUMERIC (18)   NOT NULL,
    [ExShowroom]        DECIMAL (18)   NOT NULL,
    [RTO]               VARCHAR (10)   NULL,
    [Insurance]         VARCHAR (10)   NULL,
    [OnRoad]            VARCHAR (10)   NULL,
    [Color]             VARCHAR (30)   NULL,
    [ExtendedWarranty1] VARCHAR (10)   NULL,
    [ExtendedWarranty2] VARCHAR (10)   NULL,
    [ExtendedWarranty3] VARCHAR (10)   NULL,
    [Comments]          VARCHAR (2000) NULL,
    [LastUpdated]       DATETIME       NOT NULL,
    CONSTRAINT [PK_NewCarPrices] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [DealerId] ASC) WITH (FILLFACTOR = 90)
);

