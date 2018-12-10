CREATE TABLE [dbo].[TC_RoleTasks] (
    [TC_RoleTaskId] BIGINT IDENTITY (1, 1) NOT NULL,
    [RoleId]        INT    NOT NULL,
    [TaskId]        INT    NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_RoleTasks_RoleId]
    ON [dbo].[TC_RoleTasks]([RoleId] ASC)
    INCLUDE([TaskId]);

