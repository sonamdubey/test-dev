CREATE TABLE [dbo].[Con_RoadTestAlbum] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RTId]         NUMERIC (18)  NOT NULL,
    [Caption]      VARCHAR (250) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Con_RoadTestAlbum_IsActive] DEFAULT ((1)) NOT NULL,
    [CategoryId]   NUMERIC (18)  NULL,
    [IsUpdated]    BIT           CONSTRAINT [DF_Con_RoadTestAlbum_IsUpdated] DEFAULT ((0)) NOT NULL,
    [HostURL]      VARCHAR (100) CONSTRAINT [DF_Con_RoadTestAlbum_HostURL] DEFAULT ('img.carwale.com') NULL,
    [IsReplicated] BIT           CONSTRAINT [DF_Con_RoadTestAlbum_IsReplicated] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Con_RoadTestAlbum] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

