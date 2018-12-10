﻿CREATE TABLE [dbo].[CarVersions_Bkp230615] (
    [ID]                   NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [Name]                 VARCHAR (50)    NULL,
    [CarModelId]           NUMERIC (18)    NOT NULL,
    [SegmentId]            NUMERIC (18)    NOT NULL,
    [BodyStyleId]          NUMERIC (18)    NOT NULL,
    [smallPic]             VARCHAR (100)   NULL,
    [largePic]             VARCHAR (100)   NULL,
    [IsDeleted]            BIT             NOT NULL,
    [Used]                 BIT             NOT NULL,
    [New]                  BIT             NOT NULL,
    [Indian]               BIT             NOT NULL,
    [Imported]             BIT             NOT NULL,
    [Futuristic]           BIT             NOT NULL,
    [Classic]              BIT             NOT NULL,
    [Modified]             BIT             NOT NULL,
    [ReviewRate]           DECIMAL (18, 2) NULL,
    [ReviewCount]          NUMERIC (18)    NULL,
    [Looks]                DECIMAL (18, 2) NULL,
    [Performance]          DECIMAL (18, 2) NULL,
    [Comfort]              DECIMAL (18, 2) NULL,
    [ValueForMoney]        DECIMAL (18, 2) NULL,
    [FuelEconomy]          DECIMAL (18, 2) NULL,
    [SubSegmentId]         NUMERIC (18)    NULL,
    [CarFuelType]          TINYINT         NULL,
    [CarTransmission]      TINYINT         NULL,
    [VCreatedOn]           DATETIME        NULL,
    [VUpdatedBy]           NUMERIC (18)    NULL,
    [VUpdatedOn]           DATETIME        NULL,
    [IsReplicated]         BIT             NULL,
    [HostURL]              VARCHAR (100)   NULL,
    [ReplacedByVersionId]  SMALLINT        NULL,
    [comment]              VARCHAR (5000)  NULL,
    [DiscontinuationId]    NUMERIC (18)    NULL,
    [Discontinuation_date] DATETIME        NULL,
    [IsSpecsAvailable]     BIT             NULL,
    [IsSpecsExist]         BIT             NULL,
    [SpecsSummary]         VARCHAR (100)   NULL,
    [DirPath]              VARCHAR (10)    NULL,
    [MaskingName]          VARCHAR (50)    NULL,
    [vcreatedby]           NUMERIC (18)    NULL,
    [LaunchDate]           DATETIME        NULL
);

