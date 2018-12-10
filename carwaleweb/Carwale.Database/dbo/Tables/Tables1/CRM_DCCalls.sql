CREATE TABLE [dbo].[CRM_DCCalls] (
    [Id]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [DealerId]       NUMERIC (18)   NOT NULL,
    [CallType]       INT            NOT NULL,
    [CallerId]       NUMERIC (18)   NOT NULL,
    [Subject]        VARCHAR (500)  NULL,
    [ScheduledOn]    DATETIME       NOT NULL,
    [IsActionTaken]  BIT            CONSTRAINT [DF_CRM_DCCalls_IsActionTaken] DEFAULT ((0)) NULL,
    [ActionTakenId]  SMALLINT       NULL,
    [ActionTakenOn]  DATETIME       NULL,
    [ActionComments] VARCHAR (5000) NULL,
    [ActionTakenBy]  NUMERIC (18)   NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_CRM_DCCalls_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CallStartedOn]  DATETIME       NULL,
    [IsPriorityCall] BIT            CONSTRAINT [DF_CRM_DCCalls_IsPriorityCall] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CRM_DCCalls] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

