CREATE TABLE [dbo].[OldCityMaskingNames] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [CityId]         INT          NOT NULL,
    [OldMaskingName] VARCHAR (50) NULL,
    [UpdatedOn]      DATETIME     NULL,
    [UpdatedBy]      INT          NULL
);

