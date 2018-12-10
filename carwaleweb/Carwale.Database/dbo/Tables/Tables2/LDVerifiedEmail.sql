﻿CREATE TABLE [dbo].[LDVerifiedEmail] (
    [LDID]        NUMERIC (18) NOT NULL,
    [ContactTime] VARCHAR (50) NULL,
    CONSTRAINT [PK_LDVerifiedEmail] PRIMARY KEY CLUSTERED ([LDID] ASC) WITH (FILLFACTOR = 90)
);

