CREATE TABLE [dbo].[ConsumerInvoice] (
    [ID]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CPRID]              NUMERIC (18)   NOT NULL,
    [ConsumerType]       SMALLINT       NOT NULL,
    [ConsumerId]         NUMERIC (18)   NOT NULL,
    [ConsumerName]       VARCHAR (100)  NULL,
    [ConsumerEmail]      VARCHAR (100)  NULL,
    [ConsumerContactNo]  VARCHAR (100)  NULL,
    [ConsumerAddress]    VARCHAR (150)  NULL,
    [PaymentMode]        VARCHAR (100)  NULL,
    [PaymentModeDetails] VARCHAR (200)  NULL,
    [Amount]             NUMERIC (18)   NOT NULL,
    [PackageName]        VARCHAR (500)  NULL,
    [PackageDetails]     VARCHAR (2500) NULL,
    [EntryDateTime]      DATETIME       NOT NULL,
    [IsActive]           BIT            CONSTRAINT [DF_ConsumerInvoice_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ConsumerInvoice] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ConsumerInvoice__CPRID]
    ON [dbo].[ConsumerInvoice]([CPRID] ASC)
    INCLUDE([ID]);

