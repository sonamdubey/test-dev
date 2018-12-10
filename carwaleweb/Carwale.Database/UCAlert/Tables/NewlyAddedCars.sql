﻿CREATE TABLE [UCAlert].[NewlyAddedCars] (
    [ProfileId]      VARCHAR (50)  NOT NULL,
    [SellerType]     SMALLINT      NULL,
    [MakeId]         NUMERIC (18)  NULL,
    [ModelId]        NUMERIC (18)  NULL,
    [CityId]         NUMERIC (18)  NULL,
    [MakeYear]       SMALLINT      NULL,
    [Price]          NUMERIC (18)  NULL,
    [Kilometers]     NUMERIC (18)  NULL,
    [EntryDate]      DATETIME      NULL,
    [BodyStyleId]    SMALLINT      NULL,
    [FuelType]       SMALLINT      NULL,
    [TransmissionId] TINYINT       NULL,
    [Make]           VARCHAR (50)  NULL,
    [Model]          VARCHAR (50)  NULL,
    [Version]        VARCHAR (50)  NULL,
    [City]           VARCHAR (50)  NULL,
    [Color]          VARCHAR (100) NULL,
    [Seller]         VARCHAR (20)  NULL,
    [HasPhoto]       BIT           NULL,
    [LastUpdated]    DATE          NULL
);

