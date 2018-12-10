CREATE TABLE [dbo].[TC_CallsWrongUserIdRecordsBkp] (
    [TC_CallsId]       INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadId]        INT           NULL,
    [CallType]         TINYINT       NULL,
    [TC_UsersId]       INT           NULL,
    [ScheduledOn]      DATETIME      NULL,
    [IsActionTaken]    BIT           NOT NULL,
    [TC_CallActionId]  TINYINT       NULL,
    [ActionTakenOn]    DATETIME      NULL,
    [ActionComments]   VARCHAR (MAX) NULL,
    [CreatedOn]        DATETIME      NULL,
    [AlertId]          INT           NULL,
    [NextFollowUpDate] DATETIME      NULL,
    [TC_NextActionId]  SMALLINT      NULL
);

