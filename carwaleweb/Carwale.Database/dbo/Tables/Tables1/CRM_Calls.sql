CREATE TABLE [dbo].[CRM_Calls] (
    [Id]             NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]         NUMERIC (18)   NOT NULL,
    [CallType]       INT            NOT NULL,
    [IsTeam]         BIT            NOT NULL,
    [CallerId]       NUMERIC (18)   NOT NULL,
    [Subject]        VARCHAR (500)  NULL,
    [ScheduledOn]    DATETIME       NOT NULL,
    [IsActionTaken]  BIT            CONSTRAINT [DF_CRM_Calls_IsActionTaken] DEFAULT ((0)) NOT NULL,
    [ActionTakenId]  SMALLINT       NULL,
    [ActionTakenOn]  DATETIME       NULL,
    [ActionComments] VARCHAR (5000) NULL,
    [ActionTakenBy]  NUMERIC (18)   NULL,
    [CreatedOn]      DATETIME       NOT NULL,
    [CallStartedOn]  DATETIME       NULL,
    [DispType]       SMALLINT       NULL,
    [AlertId]        INT            NULL,
    [DealerId]       NUMERIC (18)   NULL,
    [CBDID]          NUMERIC (18)   NULL,
    [IsPriorityCall] BIT            CONSTRAINT [DF_CRM_Calls_IsPriorityCall] DEFAULT ((0)) NULL,
    [RightTime]      BIT            NULL,
    CONSTRAINT [PK_CRM_Calls] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_CRM_Calls_IAT_LID_ATO_AC]
    ON [dbo].[CRM_Calls]([IsActionTaken] ASC, [LeadId] ASC)
    INCLUDE([ActionTakenOn], [ActionComments]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Calls]
    ON [dbo].[CRM_Calls]([IsTeam] ASC, [CallerId] ASC)
    INCLUDE([CallType], [ScheduledOn], [IsActionTaken]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallType]
    ON [dbo].[CRM_Calls]([CallType] ASC, [IsActionTaken] ASC)
    INCLUDE([LeadId], [ActionTakenOn]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Calls_ActionTakenBy]
    ON [dbo].[CRM_Calls]([ActionTakenBy] ASC)
    INCLUDE([ActionTakenOn]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallsAlertId]
    ON [dbo].[CRM_Calls]([AlertId] ASC)
    INCLUDE([LeadId]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_Calls__LeadId]
    ON [dbo].[CRM_Calls]([LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Calls_CallType_LeadId]
    ON [dbo].[CRM_Calls]([CallType] ASC, [IsActionTaken] ASC, [ActionTakenOn] ASC, [ActionTakenBy] ASC)
    INCLUDE([LeadId]);

