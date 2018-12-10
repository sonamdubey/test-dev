CREATE TABLE [dbo].[trkt_MasterTasks] (
    [TaskNo]           INT           IDENTITY (1, 1) NOT NULL,
    [Descr]            VARCHAR (300) NOT NULL,
    [CategoryId]       SMALLINT      NULL,
    [StartDate]        DATETIME      NULL,
    [EstimatedEndDate] DATETIME      NULL,
    [TaskCreatedBy]    INT           NULL,
    [TaskCreatedOn]    DATETIME      NULL,
    [IsActive]         BIT           CONSTRAINT [DF_trk_MasterTasks_IsActive] DEFAULT ((1)) NULL,
    [TaskTypeId]       INT           NULL,
    CONSTRAINT [PK_trk_MasterTasks] PRIMARY KEY CLUSTERED ([TaskNo] ASC)
);

