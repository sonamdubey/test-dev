CREATE TABLE [dbo].[CW_PressReleases] (
    [Id]           INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]        VARCHAR (150) NOT NULL,
    [Summary]      VARCHAR (250) NOT NULL,
    [Detailed]     VARCHAR (MAX) NOT NULL,
    [AttachedFile] VARCHAR (50)  NULL,
    [ReleaseDate]  DATETIME      NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_CW_PressReleases_IsActive] DEFAULT ((1)) NOT NULL,
    [IsReplicated] BIT           DEFAULT ((1)) NULL,
    [HostURL]      VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_CW_PressReleases] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

