﻿CREATE TABLE [dbo].[CRM_CustomPriceQuotes] (
    [CRM_BasicDataId] NUMERIC (18) NOT NULL,
    [NCS_PQRefNoId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_CustomPriceQuotes] PRIMARY KEY CLUSTERED ([CRM_BasicDataId] ASC, [NCS_PQRefNoId] ASC) WITH (FILLFACTOR = 90)
);

