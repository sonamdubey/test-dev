CREATE TABLE [dbo].[CH_Logs] (
    [ID]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CallId]            NUMERIC (18)  NOT NULL,
    [TCID]              NUMERIC (18)  NOT NULL,
    [ScheduledDateTime] DATETIME      NOT NULL,
    [CalledDateTime]    DATETIME      NOT NULL,
    [ActionId]          NUMERIC (18)  NOT NULL,
    [Comments]          VARCHAR (500) NOT NULL,
    [CallStatus]        INT           NULL,
    [CallStartDateTime] DATETIME      NULL,
    CONSTRAINT [PK_CH_Logs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CH_Logs__CallId]
    ON [dbo].[CH_Logs]([CallId] ASC);

