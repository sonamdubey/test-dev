CREATE TABLE [dbo].[UsedCarDealers] (
    [DealerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_UsedCarDealers] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

