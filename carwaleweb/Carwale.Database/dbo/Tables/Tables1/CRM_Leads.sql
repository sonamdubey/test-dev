CREATE TABLE [dbo].[CRM_Leads] (
    [ID]                     NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CNS_CustId]             NUMERIC (18) NOT NULL,
    [CreatedOn]              DATETIME     NOT NULL,
    [UpdatedOn]              DATETIME     NOT NULL,
    [LeadStageId]            SMALLINT     NOT NULL,
    [Priority]               SMALLINT     CONSTRAINT [DF_CRM_Leads_Priority] DEFAULT ((0)) NOT NULL,
    [Owner]                  NUMERIC (18) NOT NULL,
    [LeadStatusId]           NUMERIC (18) CONSTRAINT [DF_CRM_Leads_LeadStatusId] DEFAULT ((-1)) NOT NULL,
    [CreatedOnDatePart]      DATETIME     CONSTRAINT [DF_CRM_Leads_CreatedOnDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NOT NULL,
    [IsSatisfied]            BIT          NULL,
    [LeadVerifier]           NUMERIC (18) NULL,
    [GroupId]                SMALLINT     CONSTRAINT [DF_CRM_Leads_GroupId] DEFAULT ((3)) NULL,
    [IsVisitedDealer]        BIT          NULL,
    [SchedulingSlot]         SMALLINT     NULL,
    [LeadScore]              FLOAT (53)   NULL,
    [LastConnectedStatus]    SMALLINT     NULL,
    [LatestStatus]           BIT          NULL,
    [UpdatedOnLatestStatus]  DATETIME     NULL,
    [UpdatedOnLastConStatus] DATETIME     NULL,
    [LatestCallId]           BIGINT       NULL,
    [LastConnectedCallId]    BIGINT       NULL,
    [LeadScoreVersion]       SMALLINT     CONSTRAINT [DF_CRM_Leads_LeadScoreVersion] DEFAULT ((1)) NULL,
    [CCLeadScore]            FLOAT (53)   NULL,
    [PreVerLeadScore]        FLOAT (53)   NULL,
    [LeadProductType]        SMALLINT     NULL,
    CONSTRAINT [PK_CNS_Leads] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Leads_CNS_CustId]
    ON [dbo].[CRM_Leads]([CNS_CustId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Leads_CreatedOnDatePart]
    ON [dbo].[CRM_Leads]([CreatedOnDatePart] ASC)
    INCLUDE([ID], [CNS_CustId], [LeadStatusId]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_Leads__LeadStageId__Owner__GroupId]
    ON [dbo].[CRM_Leads]([LeadStageId] ASC, [Owner] ASC, [GroupId] ASC)
    INCLUDE([ID], [CreatedOn], [Priority]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_Leads_LeadStatusId]
    ON [dbo].[CRM_Leads]([LeadStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_Leads__GroupId]
    ON [dbo].[CRM_Leads]([GroupId] ASC)
    INCLUDE([CNS_CustId], [LeadStageId], [Owner], [LeadStatusId], [CreatedOnDatePart]);

