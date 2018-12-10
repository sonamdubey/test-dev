CREATE TABLE [dbo].[DLS_FiatCompList] (
    [Id]           NUMERIC (18) NOT NULL,
    [CarVersionId] NUMERIC (18) NOT NULL,
    [CityId]       NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DLS_FiatCompList] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

