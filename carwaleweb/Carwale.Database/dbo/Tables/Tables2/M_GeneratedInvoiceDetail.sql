CREATE TABLE [dbo].[M_GeneratedInvoiceDetail] (
    [Id]                   INT             IDENTITY (1, 1) NOT NULL,
    [InvoiceId]            INT             NULL,
    [PackageId]            INT             NOT NULL,
    [ProductInvoiceAmount] NUMERIC (18, 2) NOT NULL,
    [InvoiceNumber]        VARCHAR (100)   NULL,
    [Quantity]             SMALLINT        NULL,
    [SalesDealerId]        INT             NULL,
    CONSTRAINT [PK_M_GeneratedInvoiceDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

