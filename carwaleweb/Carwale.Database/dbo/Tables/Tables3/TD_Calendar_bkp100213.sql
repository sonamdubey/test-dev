CREATE TABLE [dbo].[TD_Calendar_bkp100213] (
    [TC_TDCalendarId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]        BIGINT        NOT NULL,
    [TC_CustomerId]   BIGINT        NOT NULL,
    [TC_TDCarsId]     INT           NULL,
    [TDCarDetails]    VARCHAR (100) NULL,
    [AreaName]        VARCHAR (100) NULL,
    [ArealId]         BIGINT        NOT NULL,
    [TC_UsersId]      BIGINT        NOT NULL,
    [TC_SourceId]     SMALLINT      NOT NULL,
    [TDDate]          DATE          NULL,
    [TDStartTime]     TIME (0)      NULL,
    [TDEndTime]       TIME (0)      NULL,
    [TDStatus]        TINYINT       NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [ModifiedDate]    DATETIME      NULL,
    [ModifiedBy]      BIGINT        NULL,
    [TDDriverId]      BIGINT        NULL,
    [Comments]        VARCHAR (500) NULL,
    [TDStatusDate]    DATETIME      NULL
);

