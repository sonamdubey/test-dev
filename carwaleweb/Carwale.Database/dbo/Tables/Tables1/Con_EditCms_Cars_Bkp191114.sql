CREATE TABLE [dbo].[Con_EditCms_Cars_Bkp191114] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BasicId]         NUMERIC (18) NOT NULL,
    [MakeId]          NUMERIC (18) NOT NULL,
    [ModelId]         NUMERIC (18) NOT NULL,
    [VersionId]       NUMERIC (18) NOT NULL,
    [IsActive]        BIT          NOT NULL,
    [LastUpdatedTime] DATETIME     NULL,
    [LastUpdatedBy]   NUMERIC (18) NULL
);

