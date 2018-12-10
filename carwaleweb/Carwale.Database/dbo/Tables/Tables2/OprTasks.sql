CREATE TABLE [dbo].[OprTasks] (
    [ID]           NUMERIC (18)  NOT NULL,
    [Task]         VARCHAR (200) NOT NULL,
    [DepartmentId] NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_Table1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_OprTasks_OprDepartments] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[OprDepartments] ([ID])
);

