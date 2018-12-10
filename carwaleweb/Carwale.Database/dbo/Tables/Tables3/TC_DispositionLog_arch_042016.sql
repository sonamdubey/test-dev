CREATE TABLE [dbo].[TC_DispositionLog_arch_042016] (
    [TC_DispositionLogId]    INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadDispositionId]   TINYINT       NULL,
    [InqOrLeadId]            BIGINT        NULL,
    [TC_DispositionItemId]   TINYINT       NULL,
    [EventCreatedOn]         DATETIME      NOT NULL,
    [EventOwnerId]           INT           NULL,
    [TC_LeadId]              INT           NULL,
    [LeadOwnerId]            INT           NULL,
    [NewLeadOwnerId]         INT           NULL,
    [TC_ActionApplicationId] INT           NULL,
    [DispositionReason]      VARCHAR (200) NULL,
    [TC_SubDispositionId]    INT           NULL
);

