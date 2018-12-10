CREATE TABLE [SC].[CarWaleDailyTrackerTypes] (
    [TrackerType]   SMALLINT      IDENTITY (1, 1) NOT NULL,
    [TrackerDescr]  VARCHAR (100) NULL,
    [TrackerSource] VARCHAR (30)  NULL,
    [IsActive]      BIT           NULL,
    [CreatedOn]     DATETIME      NULL,
    [UpdatedOn]     DATETIME      NULL,
    CONSTRAINT [PK__CarWaleD__913EFAE71952CDF4] PRIMARY KEY CLUSTERED ([TrackerType] ASC)
);

