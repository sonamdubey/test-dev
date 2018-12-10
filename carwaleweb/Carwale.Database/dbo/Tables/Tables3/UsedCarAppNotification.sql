CREATE TABLE [dbo].[UsedCarAppNotification] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [GCMRegId]              VARCHAR (250) NOT NULL,
    [Content]               VARCHAR (200) NOT NULL,
    [Url]                   VARCHAR (500) NOT NULL,
    [OSType]                SMALLINT      NOT NULL,
    [NotificationType]      INT           NOT NULL,
    [UsedCarNotificationId] TINYINT       NOT NULL,
    [IMEICode]              VARCHAR (50)  NOT NULL,
    [Title]                 VARCHAR (50)  NULL,
    [EntryDate]             DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([GCMRegId] ASC)
);

