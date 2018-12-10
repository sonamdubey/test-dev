CREATE TABLE [dbo].[TC_AppNotificationsLog] (
    [Id]                   INT      IDENTITY (1, 1) NOT NULL,
    [TC_UserId]            INT      NULL,
    [RecordId]             INT      NULL,
    [RecordType]           SMALLINT NULL,
    [NotificationDateTime] DATETIME NULL,
    [SentDateTime]         DATETIME NULL
);

