CREATE TABLE [SC].[RevDailyTracker] (
    [TrackerTypeId] SMALLINT      NULL,
    [TrackerType]   VARCHAR (100) NULL,
    [TrackerCount]  BIGINT        NULL,
    [TrackerDate]   DATE          NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF__RevDailyTrac__Creat__6982F3EA] DEFAULT (getdate()) NULL
);

