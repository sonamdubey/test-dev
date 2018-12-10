CREATE TABLE [dbo].[DCRM_SalesMeeting] (
    [Id]                       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SalesDealerId]            NUMERIC (18)   NULL,
    [DealerId]                 NUMERIC (18)   NOT NULL,
    [ScheduledBy]              INT            NULL,
    [ScheduledTo]              INT            NULL,
    [ScheduledDate]            DATETIME       NULL,
    [IsActionTaken]            BIT            NULL,
    [ActionTakenOn]            DATETIME       NULL,
    [ActionTakenBy]            INT            NULL,
    [CreatedOn]                DATETIME       CONSTRAINT [DF_DCRM_SalesMeeting_CreatedOn] DEFAULT (getdate()) NULL,
    [ActionComments]           VARCHAR (1000) NULL,
    [DealerStatus]             SMALLINT       NULL,
    [MeetingType]              SMALLINT       NULL,
    [MeetDecisionMaker]        BIT            NULL,
    [RateDealerMeeting]        SMALLINT       NULL,
    [MeetingDate]              DATETIME       NULL,
    [DealerType]               SMALLINT       NULL,
    [DecisionMakerName]        VARCHAR (50)   NULL,
    [IsFromMobile]             BIT            CONSTRAINT [DF_DCRM_SalesMeeting_IsFromMobile] DEFAULT ((0)) NULL,
    [MeetingMode]              SMALLINT       NULL,
    [CarsSoldOverall]          INT            NULL,
    [CarsSoldThroughCarwale]   INT            NULL,
    [FromCarsSoldBetween]      DATETIME       NULL,
    [ToCarsSoldBetween]        DATETIME       NULL,
    [DecisionMakerDesignation] VARCHAR (50)   NULL,
    [DecisionMakerPhoneNo]     VARCHAR (20)   NULL,
    [DecisionMakerEmail]       VARCHAR (50)   NULL,
    [SourceId]                 INT            DEFAULT ((1)) NULL,
    CONSTRAINT [PK_DCRM_SalesMeeting] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [DCRM_SalesMeeting_ActionTakenBy]
    ON [dbo].[DCRM_SalesMeeting]([ActionTakenBy] ASC)
    INCLUDE([Id], [DealerId], [MeetingDate]);


GO
CREATE NONCLUSTERED INDEX [DCRM_SalesMeeting_DealerId_ActionTakenBy]
    ON [dbo].[DCRM_SalesMeeting]([DealerId] ASC, [ActionTakenBy] ASC);


GO
CREATE NONCLUSTERED INDEX [DCRM_SalesMeeting_DealerId_ActionTakenBy2]
    ON [dbo].[DCRM_SalesMeeting]([ActionTakenBy] ASC, [DealerId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-cancel,2-complete,3-postpone', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_SalesMeeting', @level2type = N'COLUMN', @level2name = N'DealerStatus';

