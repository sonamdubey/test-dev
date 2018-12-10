CREATE TABLE [dbo].[trk_SubTasks1] (
    [TaskNo]           INT           IDENTITY (1003, 1) NOT NULL,
    [MasterTaskNo]     INT           NULL,
    [Descr]            VARCHAR (300) NULL,
    [StartDate]        DATETIME      NULL,
    [EstimatedEndDate] DATETIME      NULL,
    [ActualEndDate]    DATETIME      NULL,
    [AssignedBy]       INT           NULL,
    [AssignedDate]     DATETIME      NULL,
    [IsActive]         BIT           NULL,
    [IsCompleted]      BIT           NULL,
    [Comment]          VARCHAR (MAX) NULL,
    [CompletedComment] VARCHAR (MAX) NULL,
    [ManDays]          SMALLINT      NULL,
    [CategoryId]       SMALLINT      NULL,
    CONSTRAINT [PK_trk_SubTasks] PRIMARY KEY CLUSTERED ([TaskNo] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_trk_SubTasks]
    ON [dbo].[trk_SubTasks1]([MasterTaskNo] ASC);

