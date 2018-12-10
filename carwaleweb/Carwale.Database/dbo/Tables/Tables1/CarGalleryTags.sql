CREATE TABLE [dbo].[CarGalleryTags] (
    [PhotoId] NUMERIC (18) NOT NULL,
    [TagId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CarGalleryTags] PRIMARY KEY CLUSTERED ([PhotoId] ASC, [TagId] ASC) WITH (FILLFACTOR = 90)
);

