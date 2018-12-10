﻿CREATE TABLE [dbo].[Con_IHWeeklyData] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IHId]      NUMERIC (18)  NULL,
    [IHData]    VARCHAR (500) NULL,
    [IHDataURL] VARCHAR (200) NULL,
    CONSTRAINT [PK_Con_IHWeeklyData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

