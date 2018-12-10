CREATE TABLE [dbo].[CRM_CientPendingApprovals] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ClientId]            NUMERIC (18)   NOT NULL,
    [ClientType]          SMALLINT       NOT NULL,
    [LeadId]              NUMERIC (18)   NOT NULL,
    [CBDId]               NUMERIC (18)   NOT NULL,
    [CurrentEventType]    INT            NOT NULL,
    [ChangedEventType]    INT            NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_CRM_PendingCientApprovals_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsValid]             BIT            NULL,
    [CreatedOn]           DATETIME       CONSTRAINT [DF_CRM_CientPendingApprovals_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]           DATETIME       NULL,
    [UpdatedBy]           NUMERIC (18)   NULL,
    [UpdatedByDealer]     VARCHAR (50)   NULL,
    [Comments]            VARCHAR (1000) NULL,
    [DateValue]           DATETIME       NULL,
    [IsDCApproved]        BIT            CONSTRAINT [DF_CRM_CientPendingApprovals_IsDCApproved] DEFAULT ((0)) NULL,
    [ChangedSubEventType] INT            NULL,
    [SubDispositionId]    INT            NULL,
    CONSTRAINT [PK_CRM_PendingCientApprovals] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CientPendingApprovals]
    ON [dbo].[CRM_CientPendingApprovals]([LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CientPendingApprovals_ClientId]
    ON [dbo].[CRM_CientPendingApprovals]([ClientId] ASC)
    INCLUDE([CBDId]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CientPendingApprovals_CBDId]
    ON [dbo].[CRM_CientPendingApprovals]([CBDId] ASC)
    INCLUDE([CurrentEventType]);


GO
CREATE NONCLUSTERED INDEX [ix_crm_cientpendingapprovals_CurrentEventType]
    ON [dbo].[CRM_CientPendingApprovals]([CurrentEventType] ASC, [ChangedEventType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CientPendingApprovals_IsApproved]
    ON [dbo].[CRM_CientPendingApprovals]([IsApproved] ASC, [Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CientPendingApprovals_IsDCApproved]
    ON [dbo].[CRM_CientPendingApprovals]([IsDCApproved] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Dealer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_CientPendingApprovals', @level2type = N'COLUMN', @level2name = N'ClientType';

