CREATE TABLE [dbo].[ShowRoomPhotos] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]        NUMERIC (18)  NOT NULL,
    [Thumbnail]       VARCHAR (250) NOT NULL,
    [LargeImage]      VARCHAR (250) NOT NULL,
    [ImageCategory]   TINYINT       NOT NULL,
    [isMainPhoto]     BIT           CONSTRAINT [DF_ShowRoomPhotos_isMainPhoto] DEFAULT ((0)) NOT NULL,
    [isActive]        BIT           CONSTRAINT [DF_ShowRoomPhotos_isActive] DEFAULT ((1)) NOT NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF__ShowRoomP__IsRep__518AA371] DEFAULT ((0)) NULL,
    [HostURL]         VARCHAR (100) CONSTRAINT [DF__ShowRoomP__HostU__73DFBB75] DEFAULT ('img.carwale.com') NULL,
    [DirectoryPath]   VARCHAR (100) NULL,
    [RequestDate]     DATETIME      CONSTRAINT [DF_ShowRoomPhotos_RequestDate] DEFAULT (getdate()) NULL,
    [OriginalImgPath] VARCHAR (250) NULL,
    CONSTRAINT [PK_ShowRoomPhotos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ShowRoomPhotos_DealerId]
    ON [dbo].[ShowRoomPhotos]([DealerId] ASC);

