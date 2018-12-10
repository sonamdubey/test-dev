CREATE TABLE [dbo].[Con_EditCms_Pages] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]         NUMERIC (18) NOT NULL,
    [PageName]        VARCHAR (50) NOT NULL,
    [Priority]        INT          NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Con_EditCms_Pages_IsActive] DEFAULT ((1)) NOT NULL,
    [LastUpdatedTime] DATETIME     NULL,
    [LastUpdatedBy]   NUMERIC (18) NULL,
    [RTPageId]        INT          NULL,
    [BWMigratedId]    INT          NULL,
    [BWOldBasicId]    INT          NULL,
    CONSTRAINT [PK_Con_EditCms_Pages] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Pages_BasicId]
    ON [dbo].[Con_EditCms_Pages]([BasicId] ASC, [IsActive] ASC);

