CREATE TABLE [dbo].[OprSaleExecutives] (
    [SaleExecutiveID] NUMERIC (18) NOT NULL,
    [ReportToID]      NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_OprSaleExecutives] PRIMARY KEY CLUSTERED ([SaleExecutiveID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_OprSaleExecutives_OprUsers] FOREIGN KEY ([SaleExecutiveID]) REFERENCES [dbo].[OprUsers] ([Id])
);

