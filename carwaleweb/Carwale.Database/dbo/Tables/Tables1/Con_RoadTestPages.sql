CREATE TABLE [dbo].[Con_RoadTestPages] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RTId]         NUMERIC (18)  NOT NULL,
    [PageName]     VARCHAR (100) NOT NULL,
    [MainImgPath]  VARCHAR (100) NULL,
    [Caption]      VARCHAR (200) NULL,
    [ContentPath]  VARCHAR (150) NULL,
    [Priority]     SMALLINT      NULL,
    [IsReplicated] BIT           CONSTRAINT [DF__Con_RoadT__IsRep__42485FE1] DEFAULT ((0)) NULL,
    [HostURL]      VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_Con_RoadTestPages] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

