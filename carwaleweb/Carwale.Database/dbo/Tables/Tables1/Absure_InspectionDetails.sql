CREATE TABLE [dbo].[Absure_InspectionDetails] (
    [Absure_InspectionDetailsId] INT      IDENTITY (1, 1) NOT NULL,
    [CarId]                      INT      NULL,
    [EntryDate]                  DATETIME NULL,
    [TC_UserId]                  INT      NULL,
    [PhotoSyncStartTime]         DATETIME NULL,
    [PhotoSyncCompleteTime]      DATETIME NULL,
    [DataSyncStartTime]          DATETIME NULL,
    [DataSyncCompleteTime]       DATETIME NULL,
    [InspectionStartTime]        DATETIME NULL,
    [InspectionEndTime]          DATETIME NULL,
    [ModifiedOn]                 DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Absure_InspectionDetailsId] ASC)
);

