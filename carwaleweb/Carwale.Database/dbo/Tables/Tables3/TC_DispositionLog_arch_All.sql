CREATE TABLE [dbo].[TC_DispositionLog_arch_All] (
    [TC_DispositionLogId]    INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadDispositionId]   TINYINT       NULL,
    [InqOrLeadId]            BIGINT        NULL,
    [TC_DispositionItemId]   TINYINT       NULL,
    [EventCreatedOn]         DATETIME      CONSTRAINT [DF_TC_DispositionLog_EventCreatedOn] DEFAULT (getdate()) NOT NULL,
    [EventOwnerId]           INT           NULL,
    [TC_LeadId]              INT           NULL,
    [LeadOwnerId]            INT           NULL,
    [NewLeadOwnerId]         INT           NULL,
    [TC_ActionApplicationId] INT           NULL,
    [DispositionReason]      VARCHAR (200) NULL,
    [TC_SubDispositionId]    INT           NULL,
    CONSTRAINT [PK_TC_DispositionLogId] PRIMARY KEY NONCLUSTERED ([TC_DispositionLogId] ASC),
    CONSTRAINT [DF_TC_DispositionLog_EventOwnerId] FOREIGN KEY ([EventOwnerId]) REFERENCES [dbo].[TC_Users] ([Id]),
    CONSTRAINT [DF_TC_DispositionLog_TC_LeadDisposition] FOREIGN KEY ([TC_LeadDispositionId]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DispositionLog_TC_LeadDispositionId]
    ON [dbo].[TC_DispositionLog_arch_All]([TC_LeadDispositionId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TTC_DispositionLog_TC_LeadId]
    ON [dbo].[TC_DispositionLog_arch_All]([TC_LeadId] DESC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DispositionLogarch]
    ON [dbo].[TC_DispositionLog_arch_All]([TC_DispositionLogId] ASC);

