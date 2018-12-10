CREATE TABLE [dbo].[Wallpapers] (
    [ID]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]            VARCHAR (50)  NOT NULL,
    [ModelId]          NUMERIC (18)  NOT NULL,
    [VersionId]        NUMERIC (18)  NOT NULL,
    [VersionSpecific]  BIT           NOT NULL,
    [Eight]            BIT           NOT NULL,
    [Thousand]         BIT           NOT NULL,
    [RandomString]     VARCHAR (200) NOT NULL,
    [EightFileSize]    VARCHAR (15)  NULL,
    [ThousandFileSize] VARCHAR (15)  NULL,
    [EntryDate]        DATETIME      NOT NULL,
    [IsReplicated]     BIT           CONSTRAINT [DF__Wallpaper__IsRep__56D96873] DEFAULT ((0)) NULL,
    [HostURL]          VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    [OriginalImgPath]  VARCHAR (250) NULL,
    CONSTRAINT [PK_Wallpapers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

