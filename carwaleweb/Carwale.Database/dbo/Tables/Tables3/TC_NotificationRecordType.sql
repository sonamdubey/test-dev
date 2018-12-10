CREATE TABLE [dbo].[TC_NotificationRecordType] (
    [TC_NotificationRecordTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [TypeName]                    VARCHAR (100) NULL,
    CONSTRAINT [PK_TC_NotificationRecordType] PRIMARY KEY CLUSTERED ([TC_NotificationRecordTypeId] ASC)
);

