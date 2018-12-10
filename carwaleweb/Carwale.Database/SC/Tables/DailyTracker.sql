CREATE TABLE [SC].[DailyTracker] (
    [TrackerTypeId] SMALLINT      NULL,
    [TrackerType]   VARCHAR (100) NULL,
    [TrackerCount]  BIGINT        NULL,
    [TrackerDate]   DATE          NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF__DailyTrac__Creat__6982F3EA] DEFAULT (getdate()) NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_DailyTracker]
    ON [SC].[DailyTracker]([TrackerType] ASC, [TrackerDate] ASC);

