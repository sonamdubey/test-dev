CREATE TABLE [dbo].[TC_Calls] (
    [TC_CallsId]             INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadId]              INT           NULL,
    [CallType]               TINYINT       NULL,
    [TC_UsersId]             INT           NULL,
    [ScheduledOn]            DATETIME      NULL,
    [IsActionTaken]          BIT           CONSTRAINT [DF_Tc_Calls__IsActionTaken_1] DEFAULT ((0)) NOT NULL,
    [TC_CallActionId]        TINYINT       NULL,
    [ActionTakenOn]          DATETIME      NULL,
    [ActionComments]         VARCHAR (MAX) NULL,
    [CreatedOn]              DATETIME      CONSTRAINT [DF_CRM_Calls_createdOn_1 ] DEFAULT (getdate()) NULL,
    [AlertId]                INT           NULL,
    [NextFollowUpDate]       DATETIME      NULL,
    [TC_NextActionId]        SMALLINT      NULL,
    [TC_ActionApplicationId] INT           NULL,
    [NextCallTo]             INT           NULL,
    [TC_BusinessTypeId]      TINYINT       NULL,
    [TC_NextActionDate]      DATETIME      NULL,
    CONSTRAINT [PK_Tc_Calls_id_1] PRIMARY KEY CLUSTERED ([TC_CallsId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Calls_IsActionTaken_1]
    ON [dbo].[TC_Calls]([IsActionTaken] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Calls_TC_LeadId_1]
    ON [dbo].[TC_Calls]([TC_LeadId] ASC, [IsActionTaken] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Calls_TC_UsersId_1]
    ON [dbo].[TC_Calls]([TC_UsersId] ASC);

