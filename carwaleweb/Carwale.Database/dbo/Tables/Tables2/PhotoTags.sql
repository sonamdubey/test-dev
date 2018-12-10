﻿CREATE TABLE [dbo].[PhotoTags] (
    [TagId]   NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TagName] VARCHAR (200) NULL,
    CONSTRAINT [PK_PhotoTags] PRIMARY KEY CLUSTERED ([TagId] ASC) WITH (FILLFACTOR = 90)
);

