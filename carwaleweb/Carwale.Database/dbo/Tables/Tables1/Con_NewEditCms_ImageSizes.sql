CREATE TABLE [dbo].[Con_NewEditCms_ImageSizes] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ImageId]     NUMERIC (18) NOT NULL,
    [ImageWidth]  INT          NOT NULL,
    [ImageHeight] INT          NOT NULL,
    CONSTRAINT [PK_Con_NewEditCms_ImageSizes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

