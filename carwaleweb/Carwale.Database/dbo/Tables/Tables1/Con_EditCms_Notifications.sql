CREATE TABLE [dbo].[Con_EditCms_Notifications] (
    [BasicId]          INT          NOT NULL,
    [AndroidMessageId] VARCHAR (50) NULL,
    [IOSMessageId]     VARCHAR (50) NULL,
    [SentOn]           DATETIME     CONSTRAINT [DF_Con_EditCms_Notifications_SentOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Con_EditCms_Notifications] PRIMARY KEY CLUSTERED ([BasicId] ASC)
);

