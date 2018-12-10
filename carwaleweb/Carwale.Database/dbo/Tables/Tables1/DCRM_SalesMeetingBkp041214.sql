CREATE TABLE [dbo].[DCRM_SalesMeetingBkp041214] (
    [Id]                NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SalesDealerId]     NUMERIC (18)   NULL,
    [DealerId]          NUMERIC (18)   NOT NULL,
    [ScheduledBy]       INT            NULL,
    [ScheduledTo]       INT            NULL,
    [ScheduledDate]     DATETIME       NULL,
    [IsActionTaken]     BIT            NULL,
    [ActionTakenOn]     DATETIME       NULL,
    [ActionTakenBy]     INT            NULL,
    [CreatedOn]         DATETIME       NULL,
    [ActionComments]    VARCHAR (1000) NULL,
    [DealerStatus]      SMALLINT       NULL,
    [MeetingType]       SMALLINT       NULL,
    [MeetDecisionMaker] BIT            NULL,
    [RateDealerMeeting] SMALLINT       NULL,
    [MeetingDate]       DATETIME       NULL,
    [DealerType]        SMALLINT       NULL,
    [DecisionMakerName] VARCHAR (50)   NULL,
    [IsFromMobile]      BIT            NULL,
    [MeetingMode]       SMALLINT       NULL
);

