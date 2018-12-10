CREATE TABLE [dbo].[IndividualCarViews] (
    [ICV_Id]     NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CityId]     NUMERIC (18) NOT NULL,
    [TotalViews] NUMERIC (18) NULL,
    [TotalCars]  NUMERIC (18) NULL,
    [EntryDate]  DATETIME     NOT NULL,
    CONSTRAINT [PK_IndividualCarViews] PRIMARY KEY CLUSTERED ([ICV_Id] ASC) WITH (FILLFACTOR = 90)
);

