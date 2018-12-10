CREATE TABLE [dbo].[CRM_EventLogs] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EventType]     INT          NOT NULL,
    [ItemId]        NUMERIC (18) NOT NULL,
    [EventOn]       DATETIME     NOT NULL,
    [EventBy]       NUMERIC (18) NOT NULL,
    [EventDatePart] DATETIME     CONSTRAINT [DF_CRM_EventLogs_EventDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    CONSTRAINT [PK_CRM_EventLogs] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_EventLogs]
    ON [dbo].[CRM_EventLogs]([ItemId] ASC, [EventType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_EventLogs_EventType]
    ON [dbo].[CRM_EventLogs]([EventType] ASC, [EventOn] ASC)
    INCLUDE([ItemId]);

