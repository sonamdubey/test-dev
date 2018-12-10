CREATE TABLE [dbo].[TC_AppNotification] (
    [Id]                   INT      IDENTITY (1, 1) NOT NULL,
    [TC_UserId]            INT      NULL,
    [RecordId]             INT      NULL,
    [RecordType]           SMALLINT NULL,
    [NotificationDateTime] DATETIME NULL,
    CONSTRAINT [PK_TC_AppNotification] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_AppNotification_RecordId]
    ON [dbo].[TC_AppNotification]([RecordId] ASC);

