CREATE TABLE [dbo].[Articles] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]      NUMERIC (18)  NULL,
    [VersionId]    NUMERIC (18)  NULL,
    [Path]         VARCHAR (100) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Articles_IsActive] DEFAULT (1) NOT NULL,
    [IsReplicated] BIT           DEFAULT ((1)) NULL,
    [HostURL]      VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

