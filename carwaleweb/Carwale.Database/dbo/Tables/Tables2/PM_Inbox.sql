CREATE TABLE [dbo].[PM_Inbox] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MessageId]   NUMERIC (18) NOT NULL,
    [ReceiverId]  NUMERIC (18) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_PM_Inbox_IsActive] DEFAULT ((1)) NOT NULL,
    [IsRead]      BIT          CONSTRAINT [DF_PM_Inbox_IsRead] DEFAULT ((0)) NOT NULL,
    [CreatedDate] DATETIME     CONSTRAINT [DF_PM_Inbox_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_PM_Inbox] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

