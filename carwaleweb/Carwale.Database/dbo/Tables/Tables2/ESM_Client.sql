﻿CREATE TABLE [dbo].[ESM_Client] (
    [id]     NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Client] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ESM_Client] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

