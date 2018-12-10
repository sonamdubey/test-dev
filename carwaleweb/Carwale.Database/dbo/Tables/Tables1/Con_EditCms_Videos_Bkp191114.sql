CREATE TABLE [dbo].[Con_EditCms_Videos_Bkp191114] (
    [BasicId]  NUMERIC (18)  NOT NULL,
    [VideoUrl] VARCHAR (100) NULL,
    [Views]    INT           NULL,
    [Likes]    INT           NULL,
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [IsActive] BIT           NOT NULL,
    [Duration] NUMERIC (18)  NULL,
    [VideoId]  VARCHAR (100) NULL
);

