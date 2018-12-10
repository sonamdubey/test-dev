CREATE TABLE [dbo].[CH_Calls] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CallType]      SMALLINT     NOT NULL,
    [TBCType]       SMALLINT     NOT NULL,
    [TBCID]         NUMERIC (18) NOT NULL,
    [TcId]          NUMERIC (18) NULL,
    [EventId]       NUMERIC (18) CONSTRAINT [DF_CH_Calls_EventId] DEFAULT ((-1)) NOT NULL,
    [EntryDateTime] DATETIME     NOT NULL,
    [Status]        SMALLINT     CONSTRAINT [DF_CH_Calls_Status] DEFAULT ((1)) NOT NULL,
    [IsAttended]    BIT          CONSTRAINT [DF_CH_Calls_IsAttended] DEFAULT ((0)) NULL,
    [IsFreshCall]   BIT          CONSTRAINT [DF_CH_Calls_IsFreshCall] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CH_Calls] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CH_Calls]
    ON [dbo].[CH_Calls]([CallType] ASC, [TBCType] ASC, [IsFreshCall] ASC)
    INCLUDE([TcId]);


GO
CREATE NONCLUSTERED INDEX [ix_CH_Calls_CallType_TBCType_TBCID_EventId]
    ON [dbo].[CH_Calls]([CallType] ASC, [TBCType] ASC, [TBCID] ASC, [EventId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CH_Calls_EventId]
    ON [dbo].[CH_Calls]([EventId] ASC)
    INCLUDE([CallType], [IsFreshCall]);


GO
CREATE NONCLUSTERED INDEX [IX_ch_callstbcid]
    ON [dbo].[CH_Calls]([TBCID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0(false) if call is not attended else it will be 1(True)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CH_Calls', @level2type = N'COLUMN', @level2name = N'IsAttended';

