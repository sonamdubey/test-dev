CREATE TABLE [dbo].[TempNewCarDiscount] (
    [CarVersionId]   NUMERIC (18) NOT NULL,
    [CityId]         NUMERIC (18) NOT NULL,
    [DealerDiscount] NUMERIC (18) NULL,
    [RemainingCars]  NUMERIC (18) NULL,
    CONSTRAINT [PK_TempNewCarDiscount] PRIMARY KEY CLUSTERED ([CarVersionId] ASC, [CityId] ASC) WITH (FILLFACTOR = 90)
);

