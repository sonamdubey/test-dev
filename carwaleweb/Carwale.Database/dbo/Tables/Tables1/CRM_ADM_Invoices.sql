CREATE TABLE [dbo].[CRM_ADM_Invoices] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MakeId]       NUMERIC (18) NOT NULL,
    [InvoiceNo]    VARCHAR (50) NOT NULL,
    [Status]       SMALLINT     CONSTRAINT [DF_CRM_ADM_Invoices_Status] DEFAULT ((1)) NOT NULL,
    [CreatedOn]    DATETIME     CONSTRAINT [DF_CRM_ADM_Invoices_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [InvoiceMonth] DATETIME     NULL,
    CONSTRAINT [PK_CRM_ADM_Invoices] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

