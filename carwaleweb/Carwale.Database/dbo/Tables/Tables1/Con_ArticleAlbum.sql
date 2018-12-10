﻿CREATE TABLE [dbo].[Con_ArticleAlbum] (
    [ID]      NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ARTID]   NUMERIC (18)  NOT NULL,
    [Caption] VARCHAR (250) NULL,
    CONSTRAINT [PK_Con_ArticleAlbum] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

