﻿CREATE TABLE [UCAlert].[UserCarAlertsDeleteBkp17092014] (
    [UsedCarAlert_Id] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]      BIGINT         NULL,
    [Email]           VARCHAR (100)  NOT NULL,
    [CityId]          SMALLINT       NULL,
    [City]            VARCHAR (50)   NULL,
    [CityDistance]    SMALLINT       NULL,
    [BudgetId]        VARCHAR (MAX)  NULL,
    [Budget]          VARCHAR (MAX)  NULL,
    [YearId]          VARCHAR (500)  NULL,
    [MakeYear]        VARCHAR (MAX)  NULL,
    [KmsId]           VARCHAR (500)  NULL,
    [Kms]             VARCHAR (1000) NULL,
    [MakeId]          VARCHAR (MAX)  NULL,
    [Make]            VARCHAR (MAX)  NULL,
    [modelId]         VARCHAR (500)  NULL,
    [Model]           VARCHAR (1000) NULL,
    [FuelTypeId]      VARCHAR (500)  NULL,
    [FuelType]        VARCHAR (1000) NULL,
    [BodyStyleId]     VARCHAR (500)  NULL,
    [BodyStyle]       VARCHAR (1000) NULL,
    [TransmissionId]  VARCHAR (50)   NULL,
    [Transmission]    VARCHAR (50)   NULL,
    [SellerId]        VARCHAR (50)   NULL,
    [Seller]          VARCHAR (50)   NULL,
    [EntryDateTime]   DATE           NULL,
    [LastUpdated]     DATE           NULL,
    [EntryDateTicks]  CHAR (18)      NULL,
    [IsActive]        BIT            NULL,
    [AlertFrequency]  TINYINT        NOT NULL,
    [alertUrl]        VARCHAR (MAX)  NULL,
    [IsAutomated]     BIT            NULL
);

