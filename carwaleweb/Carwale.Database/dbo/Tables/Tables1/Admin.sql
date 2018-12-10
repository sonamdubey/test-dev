﻿CREATE TABLE [dbo].[Admin] (
    [ID]       SMALLINT     NOT NULL,
    [Username] VARCHAR (10) NOT NULL,
    [Passwd]   VARCHAR (20) NOT NULL,
    [ROLE]     VARCHAR (20) CONSTRAINT [DF_Admin_ROLE] DEFAULT ('ADMIN') NOT NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

