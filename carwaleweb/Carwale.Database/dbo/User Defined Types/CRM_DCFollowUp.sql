CREATE TYPE [dbo].[CRM_DCFollowUp] AS TABLE (
    [LeadId]         VARCHAR (MAX) NULL,
    [CallType]       VARCHAR (MAX) NULL,
    [IsTeam]         VARCHAR (10)  NULL,
    [CallerId]       VARCHAR (MAX) NULL,
    [Subject]        VARCHAR (MAX) NULL,
    [ScheduledOn]    DATETIME      NULL,
    [IsActionTaken]  VARCHAR (10)  NULL,
    [ActionComments] VARCHAR (MAX) NULL,
    [ActionTakenOn]  DATETIME      NULL,
    [ActionTakenBy]  VARCHAR (MAX) NULL,
    [CreatedOn]      DATETIME      NULL,
    [DealerId]       VARCHAR (MAX) NULL,
    [CallId]         VARCHAR (MAX) NULL);

