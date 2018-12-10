CREATE TABLE [dbo].[DCRM_ServiceMeeting] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]       NUMERIC (18)  NOT NULL,
    [ScheduledBy]    INT           NULL,
    [ScheduledTo]    INT           NULL,
    [ScheduledDate]  DATETIME      NULL,
    [IsActionTaken]  BIT           DEFAULT ((0)) NULL,
    [ActionTakenOn]  DATETIME      NULL,
    [ActionTakenBy]  INT           NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_DCRM_ServiceMeeting_CreatedOn] DEFAULT (getdate()) NULL,
    [ActionComments] VARCHAR (500) NULL,
    [UserAlertId]    NUMERIC (18)  NULL
);

