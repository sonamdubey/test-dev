CREATE TABLE [dbo].[TeleCallerLog] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserId]     NUMERIC (18)  NOT NULL,
    [LoginTime]  DATETIME      NOT NULL,
    [LogoutTime] DATETIME      NULL,
    [Reason]     VARCHAR (500) NULL,
    [UpdatedOn]  DATETIME      NULL,
    [UpdatedBy]  NUMERIC (18)  NULL,
    [Type]       SMALLINT      NOT NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_TeleCallerLog_CreatedOn] DEFAULT (getdate()) NULL,
    [ReasonId]   INT           NULL,
    CONSTRAINT [PK_TeleCallerLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_TeleCallerLog_Uid_LT_CO]
    ON [dbo].[TeleCallerLog]([UserId] ASC)
    INCLUDE([LogoutTime], [CreatedOn]);


GO
CREATE NONCLUSTERED INDEX [ix_TeleCallerLog__UserId]
    ON [dbo].[TeleCallerLog]([UserId] ASC)
    INCLUDE([LoginTime], [LogoutTime]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-CRM, 2-CH', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TeleCallerLog', @level2type = N'COLUMN', @level2name = N'Type';

