CREATE TABLE [dbo].[IMG_Photos] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CategoryId]   TINYINT        NOT NULL,
    [ItemId]       INT            NULL,
    [HostUrl]      VARCHAR (250)  NULL,
    [OriginalPath] VARCHAR (250)  NULL,
    [IsProcessed]  BIT            NOT NULL,
    [ProcessedId]  BIGINT         NULL,
    [AspectRatio]  DECIMAL (5, 3) NOT NULL,
    [IsWaterMark]  BIT            NULL,
    [IsMaster]     BIT            NOT NULL,
    [IsMain]       BIT            NOT NULL,
    [EntryDate]    DATETIME       CONSTRAINT [DF_Img_Photos_EntryDate] DEFAULT (getdate()) NULL,
    [ProcessedOn]  DATETIME       NULL,
    CONSTRAINT [PK_Img_Photos] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Category of the image from Img_Category table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'CategoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Item id is the id of the item whose photo is to be uploaded', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'ItemId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HostUrl of the image ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'HostUrl';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Original path of the image', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'OriginalPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 if the image is replicated else 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'IsProcessed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id from the main image table under particular category', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'ProcessedId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'aspect ratio of the image', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'AspectRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To set watermark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'IsWaterMark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To set as main image', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'IsMain';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entry date of the uploaded image', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IMG_Photos', @level2type = N'COLUMN', @level2name = N'EntryDate';

