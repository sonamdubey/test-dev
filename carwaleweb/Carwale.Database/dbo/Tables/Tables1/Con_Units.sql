﻿CREATE TABLE [dbo].[Con_Units] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [IsActive] BIT          CONSTRAINT [DF_Con_Unit_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Con_Unit] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

