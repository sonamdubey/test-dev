CREATE TABLE [dbo].[trk_SubTasks] (
    [TaskNo]           INT           NOT NULL,
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
    [CategoryId]       SMALLINT      NULL
);

