CREATE TABLE [dbo].[Con_EditCms_ImageSizes] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ImageId]      NUMERIC (18) NOT NULL,
    [ImageWidth]   INT          NOT NULL,
    [ImageHeight]  INT          NOT NULL,
    [BWMigratedId] INT          NULL,
    [BWOldImageId] INT          NULL,
    CONSTRAINT [PK_Con_EditCms_ImageSizes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

