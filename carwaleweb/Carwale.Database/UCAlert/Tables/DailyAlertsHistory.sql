CREATE TABLE [UCAlert].[DailyAlertsHistory] (
    [UsedCarAlert_Id]     INT           NOT NULL,
    [CustomerAlert_Email] VARCHAR (100) NOT NULL,
    [alertUrl]            VARCHAR (MAX) NULL,
    [cnt]                 INT           NULL,
    [UpdatedOn]           DATETIME      NOT NULL
);

