CREATE TABLE [dbo].[CarGalleryCategories] (
    [Id]       NUMERIC (10)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_CarGalleryCategories] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

