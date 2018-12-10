CREATE TABLE [dbo].[HR_JobLocations] (
    [HR_JobLocationId] INT IDENTITY (1, 1) NOT NULL,
    [HR_JobId]         INT NOT NULL,
    [StateId]          INT NOT NULL,
    [CityId]           INT NOT NULL,
    [IsActive]         BIT NOT NULL
);

