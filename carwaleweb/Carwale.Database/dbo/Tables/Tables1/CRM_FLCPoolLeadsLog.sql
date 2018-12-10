CREATE TABLE [dbo].[CRM_FLCPoolLeadsLog] (
    [UserId]    INT      NULL,
    [StartTime] DATETIME NULL,
    [Action]    INT      NULL,
    [StopTime]  DATETIME NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_FLCPoolLeadsLog_UserId]
    ON [dbo].[CRM_FLCPoolLeadsLog]([UserId] ASC, [StopTime] ASC)
    INCLUDE([StartTime], [Action]);

