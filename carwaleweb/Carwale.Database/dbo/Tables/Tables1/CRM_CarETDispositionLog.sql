CREATE TABLE [dbo].[CRM_CarETDispositionLog] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ItemId]       NUMERIC (18) NOT NULL,
    [DispositonId] NUMERIC (18) NOT NULL,
    [Type]         SMALLINT     NOT NULL,
    [CreatedBy]    NUMERIC (18) NOT NULL,
    [CreatedOn]    DATETIME     NOT NULL,
    CONSTRAINT [PK_CRM_ETDispositionLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

