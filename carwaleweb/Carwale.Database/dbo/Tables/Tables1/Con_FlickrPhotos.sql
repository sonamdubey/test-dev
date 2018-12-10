CREATE TABLE [dbo].[Con_FlickrPhotos] (
    [CFP_Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PhotoId]       NUMERIC (18)  NOT NULL,
    [FlickrUrl]     VARCHAR (250) NOT NULL,
    [SetId]         VARCHAR (250) NULL,
    [SetName]       VARCHAR (250) NULL,
    [EntryDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_Con_FlickrPhotos] PRIMARY KEY CLUSTERED ([CFP_Id] ASC) WITH (FILLFACTOR = 90)
);

