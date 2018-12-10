CREATE TABLE [dbo].[TC_NotificationsLog] (
    [TC_NotificationsLogId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_UserId]             INT      NULL,
    [RecordId]              INT      NULL,
    [RecordType]            SMALLINT NULL,
    [NotificationDateTime]  DATETIME NULL,
    [ReadDateTime]          DATETIME NULL,
    CONSTRAINT [PK_TC_NotificationsLog] PRIMARY KEY CLUSTERED ([TC_NotificationsLogId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_NotificationsLog_RecordId]
    ON [dbo].[TC_NotificationsLog]([RecordId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_NotificationsLog_TC_UserId]
    ON [dbo].[TC_NotificationsLog]([TC_UserId] ASC);

