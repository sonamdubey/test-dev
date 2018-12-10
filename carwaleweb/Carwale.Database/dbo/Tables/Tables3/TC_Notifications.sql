CREATE TABLE [dbo].[TC_Notifications] (
    [TC_NotificationsId]   INT      IDENTITY (1, 1) NOT NULL,
    [TC_UserId]            INT      NULL,
    [RecordId]             INT      NULL,
    [RecordType]           SMALLINT NULL,
    [NotificationDateTime] DATETIME NULL,
    CONSTRAINT [PK_TC_WebNotification] PRIMARY KEY CLUSTERED ([TC_NotificationsId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Notifications_TC_UserId]
    ON [dbo].[TC_Notifications]([TC_UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Notifications_RecordId]
    ON [dbo].[TC_Notifications]([RecordId] ASC);

