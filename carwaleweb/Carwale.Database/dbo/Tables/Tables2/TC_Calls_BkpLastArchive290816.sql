CREATE TABLE [dbo].[TC_Calls_BkpLastArchive290816] (
    [TC_CallsId]             INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadId]              INT           NULL,
    [CallType]               TINYINT       NULL,
    [TC_UsersId]             INT           NULL,
    [ScheduledOn]            DATETIME      NULL,
    [IsActionTaken]          BIT           CONSTRAINT [DF_Tc_Calls__IsActionTaken] DEFAULT ((0)) NOT NULL,
    [TC_CallActionId]        TINYINT       NULL,
    [ActionTakenOn]          DATETIME      NULL,
    [ActionComments]         VARCHAR (MAX) NULL,
    [CreatedOn]              DATETIME      CONSTRAINT [DF_CRM_Calls_createdOn ] DEFAULT (getdate()) NULL,
    [AlertId]                INT           NULL,
    [NextFollowUpDate]       DATETIME      NULL,
    [TC_NextActionId]        SMALLINT      NULL,
    [TC_ActionApplicationId] INT           NULL,
    [NextCallTo]             INT           NULL,
    [TC_BusinessTypeId]      TINYINT       NULL,
    [TC_NextActionDate]      DATETIME      NULL,
    CONSTRAINT [PK_Tc_Calls_id] PRIMARY KEY CLUSTERED ([TC_CallsId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [DF_TC_Calls_TC_CallAction] FOREIGN KEY ([TC_CallActionId]) REFERENCES [dbo].[TC_CallAction] ([TC_CallActionId]),
    CONSTRAINT [DF_TC_Calls_TC_CallType] FOREIGN KEY ([CallType]) REFERENCES [dbo].[TC_CallType] ([TC_CallTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Calls_TC_LeadId]
    ON [dbo].[TC_Calls_BkpLastArchive290816]([TC_LeadId] ASC, [IsActionTaken] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Calls_IsActionTaken]
    ON [dbo].[TC_Calls_BkpLastArchive290816]([IsActionTaken] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Calls_TC_UsersId]
    ON [dbo].[TC_Calls_BkpLastArchive290816]([TC_UsersId] ASC);

