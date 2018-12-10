﻿CREATE TABLE [dbo].[LDSpecialOffer] (
    [LDS_Id]      NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LDTakerId]   NUMERIC (18)  NOT NULL,
    [VersionId]   NUMERIC (18)  NOT NULL,
    [CarName]     VARCHAR (200) NOT NULL,
    [CompModelId] NUMERIC (18)  NOT NULL,
    [CityId]      NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_LDSpecialOffer] PRIMARY KEY CLUSTERED ([LDS_Id] ASC) WITH (FILLFACTOR = 90)
);

