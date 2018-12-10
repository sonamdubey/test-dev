CREATE TABLE [dbo].[CW_TargetAchievement] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BU_Id]       SMALLINT     NOT NULL,
    [WeekNumber]  INT          NOT NULL,
    [WkStartDate] DATE         NOT NULL,
    [WkEndDate]   DATE         NOT NULL,
    [Target]      INT          NOT NULL,
    [Achieved]    INT          NULL,
    [MonthNumber] TINYINT      NOT NULL,
    [MonthName]   VARCHAR (20) NOT NULL,
    [Year]        SMALLINT     NOT NULL,
    [EntryBy]     INT          NULL,
    [EntryDate]   DATETIME     NULL,
    [UpdatedBy]   INT          NOT NULL,
    [UpdatedOn]   DATETIME     NULL,
    CONSTRAINT [PK_ES_TargetAchievement] PRIMARY KEY CLUSTERED ([BU_Id] ASC, [WeekNumber] ASC, [MonthNumber] ASC, [Year] ASC)
);

