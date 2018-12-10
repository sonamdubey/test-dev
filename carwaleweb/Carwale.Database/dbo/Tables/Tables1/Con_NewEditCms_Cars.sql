CREATE TABLE [dbo].[Con_NewEditCms_Cars] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]         NUMERIC (18) NOT NULL,
    [MakeId]          NUMERIC (18) NOT NULL,
    [ModelId]         NUMERIC (18) NOT NULL,
    [VersionId]       NUMERIC (18) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Con_NewEditCms_Cars_IsActive] DEFAULT ((1)) NOT NULL,
    [LastUpdatedTime] DATETIME     NULL,
    [LastUpdatedBy]   NUMERIC (18) NULL,
    CONSTRAINT [PK_Con_NewEditCms_Cars_Id] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

