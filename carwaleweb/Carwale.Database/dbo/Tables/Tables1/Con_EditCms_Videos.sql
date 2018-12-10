CREATE TABLE [dbo].[Con_EditCms_Videos] (
    [BasicId]      NUMERIC (18)  NOT NULL,
    [VideoUrl]     VARCHAR (100) NULL,
    [Views]        INT           NULL,
    [Likes]        INT           NULL,
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Con_EditCms_Videos_IsActive] DEFAULT ((1)) NOT NULL,
    [Duration]     NUMERIC (18)  NULL,
    [VideoId]      VARCHAR (100) NULL,
    [BWMigratedId] INT           NULL,
    [BWOldBasicId] INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Videos_BasicId_IsActive]
    ON [dbo].[Con_EditCms_Videos]([BasicId] ASC, [IsActive] ASC)
    INCLUDE([VideoUrl], [Views], [Likes], [VideoId]);

