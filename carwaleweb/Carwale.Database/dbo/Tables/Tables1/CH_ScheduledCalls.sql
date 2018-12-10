CREATE TABLE [dbo].[CH_ScheduledCalls] (
    [CallID]         NUMERIC (18)  NOT NULL,
    [CallType]       SMALLINT      NOT NULL,
    [TBCType]        SMALLINT      NOT NULL,
    [TBCID]          NUMERIC (18)  NOT NULL,
    [TCID]           NUMERIC (18)  NOT NULL,
    [TBCName]        VARCHAR (200) NOT NULL,
    [TBCEmailID]     VARCHAR (200) NOT NULL,
    [TBCCity]        VARCHAR (100) NOT NULL,
    [PrimaryContact] VARCHAR (15)  NULL,
    [OtherContacts]  NCHAR (15)    NULL,
    [TBCDateTime]    DATETIME      NOT NULL,
    [CallPriority]   SMALLINT      NOT NULL,
    [EventId]        NUMERIC (18)  CONSTRAINT [DF_CH_ScheduledCalls_EventId] DEFAULT ((-1)) NOT NULL,
    CONSTRAINT [PK_CH_ScheduledCalls] PRIMARY KEY CLUSTERED ([CallID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CH_ScheduledCalls__TCID__TBCDateTime]
    ON [dbo].[CH_ScheduledCalls]([TCID] ASC, [TBCDateTime] ASC)
    INCLUDE([CallID], [CallType], [TBCType], [TBCID], [TBCName], [TBCEmailID], [TBCCity], [CallPriority], [EventId]);

