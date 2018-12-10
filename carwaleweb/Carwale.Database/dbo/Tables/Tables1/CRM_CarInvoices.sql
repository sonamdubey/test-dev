CREATE TABLE [dbo].[CRM_CarInvoices] (
    [CBDId]     NUMERIC (18) NOT NULL,
    [InvoiceId] NUMERIC (18) NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     CONSTRAINT [DF_CRM_CarInvoices_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    [IsActive]  BIT          NULL,
    CONSTRAINT [PK_CRM_CarInvoices] PRIMARY KEY CLUSTERED ([CBDId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarInvoices_InvoiceId]
    ON [dbo].[CRM_CarInvoices]([InvoiceId] ASC)
    INCLUDE([CBDId]);

