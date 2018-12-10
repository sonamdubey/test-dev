CREATE TABLE [dbo].[cityzones_backup22042016] (
    [Id]            INT          NOT NULL,
    [ZoneName]      VARCHAR (50) NULL,
    [CityId]        INT          NULL,
    [IsActive]      BIT          NULL,
    [CreatedOn]     DATETIME     NULL,
    [LastUpdatedOn] DATETIME     NULL,
    [CreatedBy]     INT          NULL,
    [LastUpdatedBy] INT          NULL,
    [ActualCityId]  INT          NULL,
    [DisplayOrder]  INT          NULL,
    [CityType]      INT          NULL
);

