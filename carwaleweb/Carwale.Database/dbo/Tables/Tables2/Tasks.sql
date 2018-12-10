CREATE TABLE [dbo].[Tasks] (
    [ID]     NUMERIC (18)  NOT NULL,
    [Name]   VARCHAR (255) NOT NULL,
    [DeptId] NUMERIC (18)  CONSTRAINT [DF_Tasks_DeptId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Tasks_Departments] FOREIGN KEY ([DeptId]) REFERENCES [dbo].[Departments] ([ID])
);

