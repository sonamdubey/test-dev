CREATE TABLE [dbo].[DCRM_UserAlerts] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [AlertId]     NUMERIC (18)  NOT NULL,
    [DealerId]    NUMERIC (18)  NOT NULL,
    [UserId]      NUMERIC (18)  NOT NULL,
    [DueDate]     DATETIME      NOT NULL,
    [ActionDate]  DATETIME      NULL,
    [Comment]     VARCHAR (500) NULL,
    [Status]      SMALLINT      CONSTRAINT [DF_DCRM_UserAlerts_Status] DEFAULT ((1)) NOT NULL,
    [ScheduledBy] NUMERIC (18)  NOT NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_DCRM_UserAlerts_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DCRM_UserAlerts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_DCRM_UserAlerts__AlertId__DealerId__UserId__Status]
    ON [dbo].[DCRM_UserAlerts]([AlertId] ASC, [DealerId] ASC, [UserId] ASC, [Status] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_DCRM_UserAlerts__DealerId__UserId__Status]
    ON [dbo].[DCRM_UserAlerts]([DealerId] ASC, [UserId] ASC, [Status] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-New, 2-Active, 3-Closed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_UserAlerts', @level2type = N'COLUMN', @level2name = N'Status';

