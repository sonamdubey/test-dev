﻿CREATE TABLE [dbo].[CRM_ADM_MTeams] (
    [ID]      NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50) NOT NULL,
    [RefType] INT          NULL,
    CONSTRAINT [PK_CRM_ADM_MTeams] PRIMARY KEY CLUSTERED ([ID] ASC)
);

