CREATE TABLE [dbo].[Dealer_AuthorizedNewCar] (
    [DealerId] NUMERIC (18) NOT NULL,
    [MakeId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_Dealer_AuthorizedNewCar] PRIMARY KEY CLUSTERED ([DealerId] ASC, [MakeId] ASC) WITH (FILLFACTOR = 90)
);

