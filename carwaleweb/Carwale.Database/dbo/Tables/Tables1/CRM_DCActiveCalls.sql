CREATE TABLE [dbo].[CRM_DCActiveCalls] (
    [CallId]         NUMERIC (18)  NOT NULL,
    [DealerId]       NUMERIC (18)  NOT NULL,
    [CallType]       INT           NOT NULL,
    [CallerId]       NUMERIC (18)  NOT NULL,
    [ScheduledOn]    DATETIME      NOT NULL,
    [Subject]        VARCHAR (500) NULL,
    [UserId]         INT           NULL,
    [IsPriorityCall] BIT           CONSTRAINT [DF_CRM_DCActiveCalls_IsPriorityCall] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CRM_DCActiveCalls] PRIMARY KEY CLUSTERED ([CallId] ASC) WITH (FILLFACTOR = 90)
);

