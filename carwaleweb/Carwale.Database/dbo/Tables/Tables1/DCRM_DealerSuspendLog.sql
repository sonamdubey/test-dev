CREATE TABLE [dbo].[DCRM_DealerSuspendLog] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]    INT      NOT NULL,
    [StatusId]    SMALLINT NOT NULL,
    [SuspendedBy] SMALLINT NOT NULL,
    [EntryDate]   DATETIME NOT NULL,
    CONSTRAINT [PK_DCRM_DealerSuspendLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

