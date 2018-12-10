CREATE TABLE [dbo].[UP_Photos] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]           VARCHAR (200) NULL,
    [Name]            VARCHAR (200) NULL,
    [Description]     VARCHAR (500) NULL,
    [EntryDate]       DATETIME      NULL,
    [AlbumId]         NUMERIC (18)  NULL,
    [Views]           NUMERIC (18)  NULL,
    [Rating]          NUMERIC (18)  NULL,
    [Size500]         BIT           NOT NULL,
    [Size800]         BIT           NOT NULL,
    [Size1024]        BIT           NOT NULL,
    [IsActive]        BIT           NULL,
    [MarkAbuse]       BIT           NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF__UP_Photos__IsRep__564F588E] DEFAULT ((0)) NULL,
    [HostURL]         VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    [DirectoryPath]   VARCHAR (100) NULL,
    [Small]           VARCHAR (200) NULL,
    [Thumbnail]       VARCHAR (200) NULL,
    [Medium]          VARCHAR (200) NULL,
    [Large]           VARCHAR (200) NULL,
    [XL]              VARCHAR (200) NULL,
    [XXL]             VARCHAR (200) NULL,
    [StatusId]        SMALLINT      CONSTRAINT [DF_UP_Photos_STATUSID] DEFAULT ((1)) NULL,
    [OriginalImgPath] VARCHAR (300) NULL,
    [UploadPath]      VARCHAR (200) NULL,
    CONSTRAINT [PK_UP_Photos] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_UP_Photos_AlbumId_IsActive]
    ON [dbo].[UP_Photos]([AlbumId] ASC, [IsActive] ASC);

