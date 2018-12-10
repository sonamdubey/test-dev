﻿CREATE TABLE [dbo].[NCS_ProcessingFee] (
    [ID]          NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FAID]        NUMERIC (18)    NOT NULL,
    [StartPrice]  DECIMAL (18, 2) NOT NULL,
    [EndPrice]    DECIMAL (18, 2) NOT NULL,
    [Fee]         DECIMAL (18, 2) NOT NULL,
    [LastUpdated] DATETIME        NOT NULL,
    CONSTRAINT [PK_NCS_ProcessingFee1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

