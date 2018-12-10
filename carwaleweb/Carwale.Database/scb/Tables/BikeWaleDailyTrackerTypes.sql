CREATE TABLE [scb].[BikeWaleDailyTrackerTypes] (
    [TrackerType]  TINYINT      IDENTITY (1, 1) NOT NULL,
    [TrackerDescr] VARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([TrackerType] ASC)
);

