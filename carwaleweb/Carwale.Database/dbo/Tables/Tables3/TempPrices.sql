CREATE TABLE [dbo].[TempPrices] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CityId]    NUMERIC (18) NOT NULL,
    [VersionId] NUMERIC (18) NOT NULL,
    [Price]     NUMERIC (18) NULL,
    [RTO]       NUMERIC (18) NULL,
    [Insurance] NUMERIC (18) NULL,
    CONSTRAINT [PK_TempPrices] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

