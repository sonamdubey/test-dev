CREATE TABLE [dbo].[IMG_CategoryResizing_Bkp191114] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CategoryId]  INT          NULL,
    [Width]       INT          NULL,
    [Height]      INT          NULL,
    [IsCrop]      BIT          NULL,
    [IsWatermark] BIT          NULL,
    [ImageProp]   VARCHAR (50) NULL
);

