CREATE TABLE [dbo].[CRM_CarETDispositions] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemId]       NUMERIC (18) NOT NULL,
    [DispositonId] NUMERIC (18) NOT NULL,
    [Type]         SMALLINT     NOT NULL,
    [CreatedBy]    NUMERIC (18) NOT NULL,
    [CreatedOn]    DATETIME     NOT NULL,
    CONSTRAINT [PK_CRM_CarETDispositions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarETDispositions_ItemId]
    ON [dbo].[CRM_CarETDispositions]([ItemId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarETDispositions_Type]
    ON [dbo].[CRM_CarETDispositions]([Type] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarETDispositions_DispositonId]
    ON [dbo].[CRM_CarETDispositions]([DispositonId] ASC);

