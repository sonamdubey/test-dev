﻿CREATE TABLE [dbo].[OLM_SCKilometersRun] (
    [Id]    NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Text]  VARCHAR (50) NOT NULL,
    [Value] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_OLM_SCKilometersRun] PRIMARY KEY CLUSTERED ([Id] ASC)
);

