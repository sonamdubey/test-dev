CREATE TABLE [dbo].[trk_MasterTasks] (
    [TaskNo]           INT           NOT NULL,
    [Descr]            VARCHAR (300) NULL,
    [CategoryId]       SMALLINT      NULL,
    [StartDate]        DATETIME      NULL,
    [EstimatedEndDate] DATETIME      NULL,
    [TaskCreatedBy]    INT           NULL,
    [TaskCreatedOn]    DATETIME      NULL,
    [IsActive]         BIT           NULL,
    [TaskTypeId]       INT           NULL,
    [createdby]        VARCHAR (100) NULL,
    [category]         VARCHAR (100) NULL
);

