CREATE TABLE [dbo].[CRM_FLCDailyPerformers] (
    [ID]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [UserId]      INT          NULL,
    [Executive]   VARCHAR (50) NULL,
    [GroupType]   TINYINT      NULL,
    [Performance] FLOAT (53)   NULL,
    [DayRank]     SMALLINT     NULL,
    [CreatedOn]   DATETIME     DEFAULT (getdate()) NULL,
    [ReportDate]  DATE         NULL
);

