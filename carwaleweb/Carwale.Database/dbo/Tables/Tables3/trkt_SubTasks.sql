CREATE TABLE [dbo].[trkt_SubTasks] (
    [TaskNo]           INT           IDENTITY (1, 1) NOT NULL,
    [MasterTaskNo]     INT           NULL,
    [Descr]            VARCHAR (300) NOT NULL,
    [StartDate]        DATETIME      NOT NULL,
    [EstimatedEndDate] DATETIME      NULL,
    [ActualEndDate]    DATETIME      NULL,
    [AssignedBy]       INT           NULL,
    [AssignedDate]     DATETIME      NULL,
    [IsActive]         BIT           NULL,
    [IsCompleted]      BIT           CONSTRAINT [DF_trk_SubTasks_IsCompleted] DEFAULT ((0)) NULL,
    [Comment]          VARCHAR (MAX) NULL,
    [CompletedComment] VARCHAR (MAX) NULL,
    [Assignedto]       SMALLINT      NULL
);

