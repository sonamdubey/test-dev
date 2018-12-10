CREATE TABLE [dbo].[DCRM_ADM_MappedUsers] (
    [Id]             INT                 IDENTITY (1, 1) NOT NULL,
    [OprUserId]      INT                 NOT NULL,
    [BusinessUnitId] INT                 NOT NULL,
    [IsActive]       BIT                 NOT NULL,
    [NodeRec]        [sys].[hierarchyid] NULL,
    [UserLevel]      AS                  ([NodeRec].[GetLevel]()) PERSISTED,
    [NodeCode]       AS                  ([NodeRec].[ToString]()) PERSISTED,
    [MappedBy]       INT                 NOT NULL,
    [MappedOn]       DATETIME            NOT NULL,
    [SuspendedBy]    INT                 NULL,
    [SuspendedOn]    DATETIME            NULL,
    [TransferBy]     INT                 NULL,
    [TransferOn]     DATETIME            NULL,
    [AliasUserId]    INT                 NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_DCRM_ADM_MappedUsers_OprUserId]
    ON [dbo].[DCRM_ADM_MappedUsers]([OprUserId] ASC);

