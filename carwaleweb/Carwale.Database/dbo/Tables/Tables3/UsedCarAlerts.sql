CREATE TABLE [dbo].[UsedCarAlerts] (
    [ID]             NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]     NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_CustomerId] DEFAULT ((-1)) NOT NULL,
    [CityId]         NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_CityId] DEFAULT ((-1)) NOT NULL,
    [CityDistance]   NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_CityDistance] DEFAULT ((-1)) NOT NULL,
    [BodyStyleId]    NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_BodyStyleId] DEFAULT ((-1)) NOT NULL,
    [SegmentId]      NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_SegmentId] DEFAULT ((-1)) NOT NULL,
    [MakeId]         VARCHAR (3000) CONSTRAINT [DF_UsedCarAlerts_MakeId] DEFAULT ((-1)) NOT NULL,
    [ModelId]        VARCHAR (3000) CONSTRAINT [DF_UsedCarAlerts_ModelId] DEFAULT ((-1)) NOT NULL,
    [VersionId]      VARCHAR (3000) CONSTRAINT [DF_UsedCarAlerts_VersionId] DEFAULT ((-1)) NOT NULL,
    [FromYear]       NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_FromYear] DEFAULT ((-1)) NOT NULL,
    [ToYear]         NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_ToYear] DEFAULT ((-1)) NOT NULL,
    [FromPrice]      NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_FromPrice] DEFAULT ((-1)) NOT NULL,
    [ToPrice]        NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_ToPrice] DEFAULT ((-1)) NOT NULL,
    [FromKm]         NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_FromKm] DEFAULT ((-1)) NOT NULL,
    [ToKm]           NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_ToKm] DEFAULT ((-1)) NOT NULL,
    [SellerType]     NUMERIC (18)   CONSTRAINT [DF_UsedCarAlerts_SellerType] DEFAULT ((-1)) NOT NULL,
    [IsActive]       BIT            CONSTRAINT [DF_UsedCarAlerts_IsActive] DEFAULT (1) NOT NULL,
    [EntryDateTime]  DATETIME       NOT NULL,
    [EntryDateTicks] CHAR (18)      NOT NULL,
    CONSTRAINT [PK_UsedCarAlerts] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

