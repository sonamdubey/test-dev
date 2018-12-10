CREATE TABLE [dbo].[CarGalleryPhotos] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]      NUMERIC (10)   NOT NULL,
    [Title]           VARCHAR (200)  NOT NULL,
    [Description]     VARCHAR (1000) NULL,
    [PhotoURL]        VARCHAR (50)   NOT NULL,
    [ModelId]         NUMERIC (18)   NOT NULL,
    [VersionId]       NUMERIC (18)   NOT NULL,
    [EntryDate]       DATETIME       NOT NULL,
    [IsActive]        BIT            NOT NULL,
    [IsReplicated]    BIT            CONSTRAINT [DF__CarGaller__IsRep__6838FE9F] DEFAULT ((0)) NULL,
    [HostURL]         VARCHAR (100)  DEFAULT ('img.carwale.com') NULL,
    [OriginalImgPath] VARCHAR (150)  NULL,
    CONSTRAINT [PK_CarGalleryPhotos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

