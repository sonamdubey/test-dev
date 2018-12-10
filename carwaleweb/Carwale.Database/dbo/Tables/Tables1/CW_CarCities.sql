CREATE TABLE [dbo].[CW_CarCities] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CW_CityId]   NUMERIC (18) NOT NULL,
    [SpokeCityId] NUMERIC (18) NOT NULL,
    [CatId]       NUMERIC (18) NULL,
    [UpdatedBy]   INT          NULL,
    [UpdatedOn]   DATETIME     NULL,
    [EntryDate]   DATETIME     DEFAULT (getdate()) NULL,
    [IsActive]    BIT          DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CW_CarCities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

