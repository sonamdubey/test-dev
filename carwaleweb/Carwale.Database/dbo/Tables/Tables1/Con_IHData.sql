CREATE TABLE [dbo].[Con_IHData] (
    [Id]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [WeekName]           VARCHAR (150)  NULL,
    [IH]                 VARCHAR (5000) NULL,
    [LastMonthVisits]    VARCHAR (50)   NULL,
    [LastWeekVisits]     VARCHAR (50)   NULL,
    [PQCount]            VARCHAR (50)   NULL,
    [MostResearchedCar]  VARCHAR (50)   NULL,
    [UsedCarsCount]      VARCHAR (50)   NULL,
    [ForumActivityCount] VARCHAR (50)   NULL,
    [RoadTestCars]       VARCHAR (500)  NULL,
    [CreatedOn]          DATETIME       NULL,
    [IsSent]             BIT            CONSTRAINT [DF_Con_IHData_IsSent] DEFAULT ((0)) NOT NULL,
    [SendDate]           DATETIME       NULL,
    CONSTRAINT [PK_Con_IHData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

