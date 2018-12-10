CREATE TABLE [dbo].[Con_EditCms_Pages_Bkp191114] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BasicId]         NUMERIC (18) NOT NULL,
    [PageName]        VARCHAR (50) NOT NULL,
    [Priority]        INT          NOT NULL,
    [IsActive]        BIT          NOT NULL,
    [LastUpdatedTime] DATETIME     NULL,
    [LastUpdatedBy]   NUMERIC (18) NULL,
    [RTPageId]        INT          NULL
);

