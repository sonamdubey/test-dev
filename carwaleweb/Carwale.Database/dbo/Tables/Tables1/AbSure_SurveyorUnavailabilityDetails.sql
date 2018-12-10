CREATE TABLE [dbo].[AbSure_SurveyorUnavailabilityDetails] (
    [Id]              NUMERIC (10) IDENTITY (1, 1) NOT NULL,
    [SurveyorId]      BIGINT       NULL,
    [UnavailableDate] DATE         NULL,
    [SlotId]          INT          NULL,
    [UpdatedDate]     DATETIME     NULL,
    [IsDenied]        BIT          NULL
);

