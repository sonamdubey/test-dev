CREATE TABLE [dbo].[ES_TrackerData] (
    [ES_TrackerData_Id] INT        IDENTITY (1, 1) NOT NULL,
    [ES_Trackers_Id]    INT        NULL,
    [TrackerCount]      FLOAT (53) NULL,
    [TrackerMonthYear]  DATE       NULL,
    [WeekendDate]       DATE       NULL,
    [WeekNoYear]        TINYINT    NULL,
    [WeekNoMonth]       TINYINT    NULL,
    [CreatedBy]         INT        NULL,
    [CreatedOn]         DATETIME   NULL,
    [Type]              TINYINT    CONSTRAINT [DF_ES_TrackerData_Type] DEFAULT ((2)) NOT NULL,
    CONSTRAINT [PK_ES_TrackerData] PRIMARY KEY CLUSTERED ([ES_TrackerData_Id] ASC)
);

