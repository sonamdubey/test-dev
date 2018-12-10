CREATE TABLE [dbo].[Absure_SurveyorActivityDetails] (
    [Absure_SurveyorActivityDetailsId] INT      IDENTITY (1, 1) NOT NULL,
    [SurveyorId]                       INT      NULL,
    [OnlineTime]                       DATETIME NULL,
    [OfflineTime]                      DATETIME NULL,
    [EntryDate]                        DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Absure_SurveyorActivityDetailsId] ASC)
);

