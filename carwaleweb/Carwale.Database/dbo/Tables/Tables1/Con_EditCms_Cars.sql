CREATE TABLE [dbo].[Con_EditCms_Cars] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]         NUMERIC (18) NOT NULL,
    [MakeId]          NUMERIC (18) NOT NULL,
    [ModelId]         NUMERIC (18) NOT NULL,
    [VersionId]       NUMERIC (18) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Con_EditCms_Cars_IsActive] DEFAULT ((1)) NOT NULL,
    [LastUpdatedTime] DATETIME     NULL,
    [LastUpdatedBy]   NUMERIC (18) NULL,
    CONSTRAINT [PK_Con_EditCms_Cars_Id] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Cars_BasicId]
    ON [dbo].[Con_EditCms_Cars]([BasicId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Cars__ModelId__IsActive]
    ON [dbo].[Con_EditCms_Cars]([ModelId] ASC, [IsActive] ASC)
    INCLUDE([BasicId], [MakeId]);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Cars__MakeId__IsActive]
    ON [dbo].[Con_EditCms_Cars]([MakeId] ASC, [IsActive] ASC)
    INCLUDE([Id], [BasicId], [ModelId]);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Cars__IsActive]
    ON [dbo].[Con_EditCms_Cars]([IsActive] ASC)
    INCLUDE([BasicId], [MakeId], [VersionId]);

