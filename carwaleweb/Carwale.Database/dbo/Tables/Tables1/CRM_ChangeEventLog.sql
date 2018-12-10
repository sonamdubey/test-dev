CREATE TABLE [dbo].[CRM_ChangeEventLog] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [PreEventType] NUMERIC (18) NOT NULL,
    [NewEventType] NUMERIC (18) NOT NULL,
    [ItemId]       NUMERIC (18) NOT NULL,
    [EventOn]      DATETIME     CONSTRAINT [DF_CRM_ChangeEventLog_EventOn] DEFAULT (getdate()) NOT NULL,
    [EventBy]      NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_ChangeEventLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

