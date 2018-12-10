CREATE TABLE [dbo].[CRM_ScheduleDCCallLog] (
    [LogId]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CallId]        NUMERIC (18) NOT NULL,
    [ScheduledBy]   NUMERIC (18) NOT NULL,
    [ScheduledDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_CRM_ScheduleDCCallLog] PRIMARY KEY CLUSTERED ([LogId] ASC)
);

