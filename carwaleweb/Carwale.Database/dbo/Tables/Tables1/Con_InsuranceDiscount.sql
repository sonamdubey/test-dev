CREATE TABLE [dbo].[Con_InsuranceDiscount] (
    [Id]       NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CityId]   NUMERIC (18)   NULL,
    [ModelId]  NUMERIC (18)   NULL,
    [Discount] NUMERIC (5, 2) NULL,
    CONSTRAINT [PK_Con_InsuranceDiscount] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

