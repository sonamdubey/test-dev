﻿CREATE TABLE [dbo].[OprDepartments] (
    [ID]   NUMERIC (18) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_oprDepartments] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

