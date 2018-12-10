CREATE TABLE [dbo].[OprRoleTasks] (
    [RoleId]  NUMERIC (18) NOT NULL,
    [TasksId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_OprRoleTasks] PRIMARY KEY CLUSTERED ([RoleId] ASC, [TasksId] ASC) WITH (FILLFACTOR = 90)
);

