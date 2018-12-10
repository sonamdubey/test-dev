CREATE TABLE [dbo].[M_GeneratedInvoice] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [TransactionId]           INT             NOT NULL,
    [PackageId]               INT             NULL,
    [InvoiceAmount]           NUMERIC (18, 2) NULL,
    [InvoiceNumber]           VARCHAR (100)   NULL,
    [EntryDate]               DATETIME        NULL,
    [GeneratedBy]             INT             NULL,
    [InvoiceName]             VARCHAR (250)   NULL,
    [InvoiceSeries]           VARCHAR (50)    NULL,
    [InvoiceSeriesNo]         INT             NULL,
    [PostTaxInvoiceAmount]    NUMERIC (18, 2) NULL,
    [Status]                  SMALLINT        DEFAULT ((2)) NULL,
    [InvoiceSeriesId]         SMALLINT        NULL,
    [InvoiceDate]             DATETIME        NULL,
    [UpdatedBy]               INT             NULL,
    [Comments]                VARCHAR (250)   NULL,
    [UsedInvoiceId]           INT             NULL,
    [UpdatedOn]               DATETIME        NULL,
    [RejectedDate]            DATETIME        NULL,
    [TextToBePrinted]         VARCHAR (250)   NULL,
    [ShowContractSummary]     BIT             DEFAULT ((0)) NOT NULL,
    [IsCleanMissionManual]    BIT             DEFAULT ((0)) NULL,
    [SourceId]                INT             NULL,
    [IsKrishiKalyanTaxManual] BIT             DEFAULT ((0)) NULL,
    CONSTRAINT [PK_M_GeneratedInvoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_M_GeneratedInvoice_Status]
    ON [dbo].[M_GeneratedInvoice]([Status] ASC);

