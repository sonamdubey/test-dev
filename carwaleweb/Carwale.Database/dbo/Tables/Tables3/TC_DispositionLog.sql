CREATE TABLE [dbo].[TC_DispositionLog] (
    [TC_DispositionLogId]    INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadDispositionId]   TINYINT       NULL,
    [InqOrLeadId]            BIGINT        NULL,
    [TC_DispositionItemId]   TINYINT       NULL,
    [EventCreatedOn]         DATETIME      CONSTRAINT [DF_TC_DispositionLog_EventCreatedOn2] DEFAULT (getdate()) NOT NULL,
    [EventOwnerId]           INT           NULL,
    [TC_LeadId]              INT           NULL,
    [LeadOwnerId]            INT           NULL,
    [NewLeadOwnerId]         INT           NULL,
    [TC_ActionApplicationId] INT           NULL,
    [DispositionReason]      VARCHAR (200) NULL,
    [TC_SubDispositionId]    INT           NULL,
    CONSTRAINT [PK_TC_DispositionLogId2] PRIMARY KEY NONCLUSTERED ([TC_DispositionLogId] ASC),
    CONSTRAINT [DF_TC_DispositionLog_EventOwnerId2] FOREIGN KEY ([EventOwnerId]) REFERENCES [dbo].[TC_Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DispositionLog_EventOwnerId]
    ON [dbo].[TC_DispositionLog]([EventOwnerId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TTC_DispositionLog_TC_LeadId2]
    ON [dbo].[TC_DispositionLog]([TC_LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DispositionLog_TC_LeadDispositionId2]
    ON [dbo].[TC_DispositionLog]([TC_LeadDispositionId] ASC);

