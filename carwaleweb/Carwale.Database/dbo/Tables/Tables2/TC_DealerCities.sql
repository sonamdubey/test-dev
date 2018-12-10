CREATE TABLE [dbo].[TC_DealerCities] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [DealerId] INT NOT NULL,
    [CityId]   INT NOT NULL,
    [IsActive] BIT CONSTRAINT [DF_TC_DealerCities_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_DealerCities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

