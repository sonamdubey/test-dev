﻿CREATE TABLE [dbo].[RecommendCars_07102013] (
    [RecommendCarId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Makeid]            SMALLINT      NULL,
    [Makename]          VARCHAR (100) NULL,
    [Modelid]           SMALLINT      NULL,
    [Modelname]         VARCHAR (100) NULL,
    [Versionid]         INT           NULL,
    [Versionname]       VARCHAR (100) NULL,
    [Price]             BIGINT        NULL,
    [DimensionAndSpace] FLOAT (53)    NULL,
    [Comfort]           FLOAT (53)    NULL,
    [Performance]       FLOAT (53)    NULL,
    [Convenience]       FLOAT (53)    NULL,
    [Safety]            FLOAT (53)    NULL,
    [Entertainment]     FLOAT (53)    NULL,
    [Aesthetics]        FLOAT (53)    NULL,
    [SalesAndSupport]   FLOAT (53)    NULL,
    [FuelEconomy]       FLOAT (53)    NULL,
    [ChildSafety]       FLOAT (53)    NULL,
    [Powerwindows]      VARCHAR (50)  NULL,
    [ABS]               FLOAT (53)    NULL,
    [CentralLocking]    VARCHAR (50)  NULL,
    [Displacement]      FLOAT (53)    NULL,
    [Power]             VARCHAR (60)  NULL,
    [Gearbox]           VARCHAR (60)  NULL,
    [FinalScore]        FLOAT (53)    NULL,
    [AirBags]           VARCHAR (100) NULL
);

