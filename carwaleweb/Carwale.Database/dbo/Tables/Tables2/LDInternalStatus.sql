﻿CREATE TABLE [dbo].[LDInternalStatus] (
    [ID]     SMALLINT      NOT NULL,
    [Status] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_LDInternalStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

