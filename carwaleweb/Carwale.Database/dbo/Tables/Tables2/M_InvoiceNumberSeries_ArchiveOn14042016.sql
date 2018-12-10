CREATE TABLE [dbo].[M_InvoiceNumberSeries_ArchiveOn14042016] (
    [Id]                         INT           IDENTITY (1, 1) NOT NULL,
    [InquiryPoint]               VARCHAR (150) NOT NULL,
    [InquiryPointId]             INT           NOT NULL,
    [InvoiceNoSeriesPattern]     VARCHAR (100) NOT NULL,
    [ServiceTaxCategory]         VARCHAR (100) NOT NULL,
    [AccoutingLedgerName]        VARCHAR (100) NOT NULL,
    [AccoutingVoucherName]       VARCHAR (100) NULL,
    [NameOfGroup]                VARCHAR (100) NULL,
    [CurrentInvoiceNumber]       INT           NULL,
    [CurrentInvoiceSeriesNumber] INT           NULL,
    [GroupId]                    SMALLINT      NULL
);

