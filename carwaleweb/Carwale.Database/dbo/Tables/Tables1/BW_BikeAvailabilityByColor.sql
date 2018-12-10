CREATE TABLE [dbo].[BW_BikeAvailabilityByColor] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT      NULL,
    [BikeVersionId]   INT      NULL,
    [ColorId]         INT      NULL,
    [NumOfDays]       INT      NULL,
    [IsActive]        BIT      NULL,
    [EntryDate]       DATETIME DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME NULL,
    [UpdatedBy]       INT      NULL
);

