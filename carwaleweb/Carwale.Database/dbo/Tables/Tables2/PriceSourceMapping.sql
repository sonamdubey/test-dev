CREATE TABLE [dbo].[PriceSourceMapping] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [PriceSourceId] INT      NOT NULL,
    [MakeId]        INT      NOT NULL,
    [StateId]       INT      NOT NULL,
    [CityId]        INT      NOT NULL,
    [DealerId]      INT      NULL,
    [UpdatedBy]     INT      NOT NULL,
    [UpdatedOn]     DATETIME NOT NULL,
    CONSTRAINT [PK_PriceSourceMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

