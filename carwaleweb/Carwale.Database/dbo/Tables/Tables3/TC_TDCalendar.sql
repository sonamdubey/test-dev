CREATE TABLE [dbo].[TC_TDCalendar] (
    [TC_TDCalendarId]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]               BIGINT        NOT NULL,
    [TC_CustomerId]          BIGINT        NOT NULL,
    [TC_TDCarsId]            INT           NULL,
    [TDCarDetails]           VARCHAR (100) NULL,
    [AreaName]               VARCHAR (100) NULL,
    [ArealId]                BIGINT        NOT NULL,
    [TC_UsersId]             BIGINT        NOT NULL,
    [TC_SourceId]            SMALLINT      NOT NULL,
    [TDDate]                 DATE          NULL,
    [TDStartTime]            TIME (0)      NULL,
    [TDEndTime]              TIME (0)      NULL,
    [TDStatus]               TINYINT       NULL,
    [EntryDate]              DATETIME      CONSTRAINT [DF_TC_TDCalendar_EntryDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]           DATETIME      NULL,
    [ModifiedBy]             BIGINT        NULL,
    [TDDriverId]             BIGINT        DEFAULT (NULL) NULL,
    [Comments]               VARCHAR (500) NULL,
    [TDStatusDate]           DATETIME      NULL,
    [TC_NewCarInquiriesId]   INT           NULL,
    [TDAddress]              VARCHAR (200) NULL,
    [TC_ActionApplicationId] INT           NULL,
    CONSTRAINT [PK_TC_TDCalendarId] PRIMARY KEY CLUSTERED ([TC_TDCalendarId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TDCalendar]
    ON [dbo].[TC_TDCalendar]([BranchId] ASC, [TC_TDCarsId] ASC, [TDDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TDCalendar_TC_SourceId]
    ON [dbo].[TC_TDCalendar]([TC_SourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TDCalendar_TC_CustomerId_TC_UsersId]
    ON [dbo].[TC_TDCalendar]([TC_CustomerId] ASC, [TC_UsersId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TDCalendar_TDDate_TDStatus]
    ON [dbo].[TC_TDCalendar]([TDDate] ASC, [TDStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [TC_TDCalendar_TC_NewCarInquiriesId]
    ON [dbo].[TC_TDCalendar]([TC_NewCarInquiriesId] ASC);

