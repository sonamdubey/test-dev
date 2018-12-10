CREATE TABLE [dbo].[DeletedDealers] (
    [DealerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DeletedDealers] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

