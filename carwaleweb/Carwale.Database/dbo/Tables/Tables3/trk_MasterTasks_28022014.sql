CREATE TABLE [dbo].[trk_MasterTasks_28022014] (
    [TaskNo]           INT           IDENTITY (217, 1) NOT NULL,
    [Descr]            VARCHAR (300) NULL,
    [CategoryId]       SMALLINT      NULL,
    [StartDate]        DATETIME      NULL,
    [EstimatedEndDate] DATETIME      NULL,
    [TaskCreatedBy]    INT           NULL,
    [TaskCreatedOn]    DATETIME      NULL,
    [IsActive]         BIT           NULL,
    [TaskTypeId]       INT           NULL
);

