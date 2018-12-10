﻿CREATE TABLE [dbo].[DealerCarViews] (
    [DCV_Id]     NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]   NUMERIC (18) NOT NULL,
    [TotalCars]  NUMERIC (18) NULL,
    [TotalViews] NUMERIC (18) NULL,
    [EntryDate]  DATETIME     NOT NULL,
    CONSTRAINT [PK_DealerCarViews] PRIMARY KEY CLUSTERED ([DCV_Id] ASC) WITH (FILLFACTOR = 90)
);

