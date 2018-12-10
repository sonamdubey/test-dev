CREATE TABLE [dbo].[NCD_DefinedDealerRules] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]  INT      NOT NULL,
    [MakeId]    INT      NULL,
    [ModelId]   INT      NULL,
    [VersionId] INT      NULL,
    [StateId]   INT      NULL,
    [CityId]    INT      NULL,
    [ZoneId]    INT      NULL,
    [CreatedBy] INT      NOT NULL,
    [CreatedOn] DATETIME NOT NULL,
    [IsActive]  BIT      NULL
);

