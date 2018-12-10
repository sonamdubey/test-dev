CREATE TABLE [dbo].[CRM_TargetLog] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [UserId]        INT      NOT NULL,
    [Brand]         INT      NOT NULL,
    [Type]          INT      NOT NULL,
    [Date]          DATE     NOT NULL,
    [Value]         INT      NOT NULL,
    [ActualLeads]   INT      NULL,
    [IsDeleted]     BIT      CONSTRAINT [DF_CRM_TargetLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    [ActionTakenBy] INT      NOT NULL,
    [ActionTakenOn] DATETIME NOT NULL,
    CONSTRAINT [PK_CRM_TargetLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_TargetLog]
    ON [dbo].[CRM_TargetLog]([Brand] ASC, [UserId] ASC, [Date] ASC);

