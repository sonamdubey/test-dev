CREATE TABLE [dbo].[CL_OrphanLogs] (
    [ID]               NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CalledNumber]     VARCHAR (50) NULL,
    [AgentId]          VARCHAR (50) NULL,
    [EntryDateTime]    DATETIME     NULL,
    [IsCallMatured]    BIT          NULL,
    [DialingTime]      NUMERIC (18) NULL,
    [CallStartTime]    DATETIME     NULL,
    [CallEndTime]      DATETIME     NULL,
    [TalkDuration]     NUMERIC (18) NULL,
    [AspectCallId]     VARCHAR (50) NULL,
    [DispositionType]  SMALLINT     NULL,
    [AspectLogId]      NUMERIC (18) NULL,
    [AspectSequenceId] NUMERIC (18) NULL,
    CONSTRAINT [PK_CL_OrphanLogs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

