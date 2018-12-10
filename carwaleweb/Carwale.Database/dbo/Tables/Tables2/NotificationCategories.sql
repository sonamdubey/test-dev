CREATE TABLE [dbo].[NotificationCategories] (
    [CategoryId]       INT NOT NULL,
    [ApplicationId]    INT NOT NULL,
    [PushNotification] BIT NOT NULL,
    CONSTRAINT [uq_NotificationCategories] UNIQUE NONCLUSTERED ([CategoryId] ASC, [ApplicationId] ASC)
);

