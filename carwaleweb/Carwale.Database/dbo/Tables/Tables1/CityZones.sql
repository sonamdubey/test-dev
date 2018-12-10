CREATE TABLE [dbo].[CityZones] (
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
    [CityType]      INT          NULL,
    CONSTRAINT [PK_CityZones] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Actual CityId From Cities Table With Same Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CityZones', @level2type = N'COLUMN', @level2name = N'ActualCityId';

