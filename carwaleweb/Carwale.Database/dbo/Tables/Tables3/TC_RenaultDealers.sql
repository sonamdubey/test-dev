CREATE TABLE [dbo].[TC_RenaultDealers] (
    [DealerId] INT NULL
);


GO
CREATE CLUSTERED INDEX [ix_TC_RenaultDealers_DealerId]
    ON [dbo].[TC_RenaultDealers]([DealerId] ASC);

