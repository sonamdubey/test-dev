CREATE TABLE [dbo].[IMG_AllCarPhotos] (
    [Id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [CategoryId]       INT           NULL,
    [ItemId]           INT           NULL,
    [URL]              VARCHAR (255) NULL,
    [StatusId]         INT           NULL,
    [OriginalFilename] VARCHAR (255) NULL,
    [EntryDate]        DATETIME      NULL,
    [ItemStorage]      VARCHAR (255) NULL,
    [MaxServers]       INT           NULL,
    [BWMigratedId]     INT           NULL,
    [BWOldItemId]      INT           NULL,
    [BWOldCategoryId]  INT           NULL,
    CONSTRAINT [PK_AllCarPhotos] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_IMG_AllCarPhotos]
    ON [dbo].[IMG_AllCarPhotos]([ItemId] ASC);

