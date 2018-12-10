CREATE TABLE [dbo].[DCRM_Calls] (
    [Id]            NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UserId]        NUMERIC (18)   NOT NULL,
    [DealerId]      NUMERIC (18)   NOT NULL,
    [ScheduleDate]  DATETIME       NOT NULL,
    [CalledDate]    DATETIME       NULL,
    [LastCallDate]  DATETIME       NULL,
    [ActionTakenId] INT            CONSTRAINT [DF_DCRM_Calls_ActionTakenId] DEFAULT ((2)) NOT NULL,
    [Subject]       VARCHAR (200)  NULL,
    [Comments]      VARCHAR (1500) NULL,
    [ScheduledBy]   NUMERIC (18)   NOT NULL,
    [CreatedOn]     DATETIME       CONSTRAINT [DF_DCRM_Calls_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CallType]      INT            NULL,
    [CallStatus]    SMALLINT       NULL,
    [LastComment]   VARCHAR (1500) NULL,
    [AlertId]       INT            NULL,
    [CallCategory]  INT            NULL,
    CONSTRAINT [PK_DCRM_Calls] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DCRM_Calls_UserId]
    ON [dbo].[DCRM_Calls]([UserId] ASC, [ActionTakenId] ASC, [ScheduleDate] ASC)
    INCLUDE([Id], [DealerId], [LastCallDate], [Subject], [CallType], [LastComment], [AlertId]);


GO
CREATE NONCLUSTERED INDEX [IX_DCRM_Calls_DealerId]
    ON [dbo].[DCRM_Calls]([DealerId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2-Open, 1-Closed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_Calls', @level2type = N'COLUMN', @level2name = N'ActionTakenId';

