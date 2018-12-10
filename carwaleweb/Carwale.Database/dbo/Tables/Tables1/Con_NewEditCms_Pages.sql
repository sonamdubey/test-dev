CREATE TABLE [dbo].[Con_NewEditCms_Pages] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]         NUMERIC (18) NOT NULL,
    [PageName]        VARCHAR (50) NOT NULL,
    [Priority]        INT          NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Con_NewEditCms_Pages_IsActive] DEFAULT ((1)) NOT NULL,
    [LastUpdatedTime] DATETIME     NULL,
    [LastUpdatedBy]   NUMERIC (18) NULL,
    [RTPageId]        INT          NULL,
    CONSTRAINT [PK_Con_NewEditCms_Pages] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Con_NewEditCms_Pages_Con_NewEditCms_Pages] FOREIGN KEY ([BasicId]) REFERENCES [dbo].[Con_NewEditCms_Basic] ([Id])
);

