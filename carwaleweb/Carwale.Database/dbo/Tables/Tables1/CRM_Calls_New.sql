CREATE TABLE [dbo].[CRM_Calls_New] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]         NUMERIC (18)  NOT NULL,
    [CallType]       INT           NOT NULL,
    [IsTeam]         BIT           NOT NULL,
    [CallerId]       NUMERIC (18)  NOT NULL,
    [Subject]        VARCHAR (500) NULL,
    [ScheduledOn]    DATETIME      NOT NULL,
    [IsActionTaken]  BIT           CONSTRAINT [DF_CRM_Calls_IsActionTaken1] DEFAULT ((0)) NOT NULL,
    [ActionTakenId]  SMALLINT      NULL,
    [ActionTakenOn]  DATETIME      NULL,
    [ActionComments] VARCHAR (500) NULL,
    [ActionTakenBy]  NUMERIC (18)  NULL,
    [CreatedOn]      DATETIME      NOT NULL,
    [CallStartedOn]  DATETIME      NULL,
    [DispType]       SMALLINT      NULL,
    [AlertId]        INT           NULL,
    [DealerId]       NUMERIC (18)  NULL,
    CONSTRAINT [PK_CRM_Calls1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_CRM_Calls_IAT_LID_ATO_AC1]
    ON [dbo].[CRM_Calls_New]([IsActionTaken] ASC, [LeadId] ASC)
    INCLUDE([ActionTakenOn], [ActionComments]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Calls1]
    ON [dbo].[CRM_Calls_New]([IsTeam] ASC, [CallerId] ASC)
    INCLUDE([CallType], [ScheduledOn], [IsActionTaken]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallType1]
    ON [dbo].[CRM_Calls_New]([CallType] ASC, [IsActionTaken] ASC)
    INCLUDE([LeadId], [ActionTakenOn]);

