CREATE TABLE [dbo].[PQCityGroups] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [CityName]      VARCHAR (50) NOT NULL,
    [CityId]        INT          NOT NULL,
    [IsActive]      BIT          NOT NULL,
    [CreatedOn]     DATETIME     DEFAULT (getdate()) NULL,
    [LastUpdatedOn] DATETIME     DEFAULT (getdate()) NULL,
    [CreatedBy]     INT          NULL,
    [LastUpdatedBy] INT          NULL,
    [DisplayOrder]  INT          NOT NULL,
    [GroupMasterId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

