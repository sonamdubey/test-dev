CREATE TABLE [dbo].[CH_TeleCallers] (
    [TCID]             NUMERIC (18) NOT NULL,
    [ScheduledCalls]   NUMERIC (18) CONSTRAINT [DF_CH_TeleCallers_ScheduledCalls] DEFAULT ((0)) NOT NULL,
    [CallsMade]        NUMERIC (18) CONSTRAINT [DF_CH_TeleCallers_CallsMade] DEFAULT ((0)) NOT NULL,
    [CallsTerminated]  NUMERIC (18) CONSTRAINT [DF_CH_TeleCallers_CallsTerminated] DEFAULT ((0)) NOT NULL,
    [LastActivityTime] DATETIME     CONSTRAINT [DF_CH_TeleCallers_LoggedIn] DEFAULT ((0)) NOT NULL,
    [IsReady]          BIT          CONSTRAINT [DF_CH_TeleCallers_IsReady] DEFAULT ((1)) NULL,
    [IsActive]         BIT          CONSTRAINT [DF_CH_TeleCallers_IsActve] DEFAULT ((1)) NULL,
    [LastLeadTime]     DATETIME     NULL,
    [IsNew]            BIT          NULL,
    [IsActiveLogin]    BIT          NULL,
    CONSTRAINT [PK_CH_TeleCallers] PRIMARY KEY CLUSTERED ([TCID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CH_TeleCallers]
    ON [dbo].[CH_TeleCallers]([IsActiveLogin] ASC, [TCID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1= ready for taking new calls, 0=not ready', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CH_TeleCallers', @level2type = N'COLUMN', @level2name = N'IsReady';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Agent is active else its inactive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CH_TeleCallers', @level2type = N'COLUMN', @level2name = N'IsActive';

