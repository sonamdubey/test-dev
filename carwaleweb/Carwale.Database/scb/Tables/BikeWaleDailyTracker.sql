CREATE TABLE [scb].[BikeWaleDailyTracker] (
    [TrackerType]  TINYINT  NOT NULL,
    [TrackerCount] BIGINT   NULL,
    [TrackerDate]  DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([TrackerType] ASC, [TrackerDate] ASC)
);

