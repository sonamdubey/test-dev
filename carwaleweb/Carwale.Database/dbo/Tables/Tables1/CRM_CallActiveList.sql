CREATE TABLE [dbo].[CRM_CallActiveList] (
    [CallId]         NUMERIC (18)  NOT NULL,
    [LeadId]         NUMERIC (18)  NOT NULL,
    [CallType]       INT           NOT NULL,
    [IsTeam]         BIT           NOT NULL,
    [CallerId]       NUMERIC (18)  NOT NULL,
    [ScheduledOn]    DATETIME      NOT NULL,
    [Subject]        VARCHAR (500) NULL,
    [AlertId]        INT           NULL,
    [DealerId]       NUMERIC (18)  NULL,
    [CBDId]          NUMERIC (18)  NULL,
    [UserId]         INT           NULL,
    [IsPriorityCall] BIT           CONSTRAINT [DF_CRM_CallActiveList_IsPriorityCall] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CRM_ActiveCalls] PRIMARY KEY CLUSTERED ([CallId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_AlertId]
    ON [dbo].[CRM_CallActiveList]([AlertId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_2]
    ON [dbo].[CRM_CallActiveList]([IsTeam] ASC, [UserId] ASC, [CallType] ASC, [ScheduledOn] ASC)
    INCLUDE([CallId], [LeadId], [CallerId], [AlertId]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CallActiveList__LeadId]
    ON [dbo].[CRM_CallActiveList]([LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_CallerId]
    ON [dbo].[CRM_CallActiveList]([CallerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_LeadId]
    ON [dbo].[CRM_CallActiveList]([LeadId] ASC, [IsTeam] ASC, [UserId] ASC, [CallType] ASC, [CallerId] ASC, [ScheduledOn] ASC)
    INCLUDE([CallId]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_CallId]
    ON [dbo].[CRM_CallActiveList]([IsTeam] ASC, [CallerId] ASC, [CallType] ASC)
    INCLUDE([CallId]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_Dealerid]
    ON [dbo].[CRM_CallActiveList]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_CallType]
    ON [dbo].[CRM_CallActiveList]([CallType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CallActiveList_ScheduledOn]
    ON [dbo].[CRM_CallActiveList]([ScheduledOn] ASC);

