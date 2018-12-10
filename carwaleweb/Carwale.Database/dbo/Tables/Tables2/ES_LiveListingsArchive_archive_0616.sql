CREATE TABLE [dbo].[ES_LiveListingsArchive_archive_0616] (
    [ProfileID]       VARCHAR (50) NOT NULL,
    [Action]          CHAR (6)     NULL,
    [ActionType]      TINYINT      NULL,
    [LastUpdateTime]  DATETIME     NULL,
    [IsSynced]        BIT          NULL,
    [SyncTime]        DATETIME     NULL,
    [ArchiveDateTime] DATETIME     CONSTRAINT [DF_ES_LiveListingsArchive] DEFAULT (getdate()) NULL
);

