CREATE TABLE [dbo].[ExtraPricings] (
    [Id]                      NUMERIC (18) NOT NULL,
    [NewCarPricingId]         NUMERIC (18) NOT NULL,
    [ExtraPricingComponentId] NUMERIC (18) NOT NULL,
    [Price]                   DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_ExtraPricings] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

