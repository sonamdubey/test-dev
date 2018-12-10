CREATE TABLE [dbo].[UsedCarAppNotificationLog] (
    [Id]                    INT          IDENTITY (1, 1) NOT NULL,
    [IMEICode]              VARCHAR (50) NOT NULL,
    [LastNotified]          DATETIME     NOT NULL,
    [OSType]                SMALLINT     NULL,
    [UsedCarNotificationId] TINYINT      NULL,
    [SentDate]              DATETIME     NULL,
    CONSTRAINT [PK_UsedCarAppNotificationLog] PRIMARY KEY CLUSTERED ([IMEICode] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UsedCarAppNotificationLog_LastNotified]
    ON [dbo].[UsedCarAppNotificationLog]([LastNotified] ASC);

