CREATE TABLE [dbo].[PM_Sent] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MessageId]       NUMERIC (18) NOT NULL,
    [SenderId]        NUMERIC (18) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_PM_Sent_IsActive] DEFAULT ((1)) NOT NULL,
    [MessageSendDate] DATETIME     CONSTRAINT [DF_PM_Sent_MessageSendDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_PM_Sent] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

