CREATE TABLE [dbo].[DCRM_SalesDealerMapping] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SalesDealerId] NUMERIC (18) NOT NULL,
    [DealerId]      NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_SalesDealerMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

