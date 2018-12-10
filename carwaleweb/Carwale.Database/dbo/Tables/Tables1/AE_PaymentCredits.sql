CREATE TABLE [dbo].[AE_PaymentCredits] (
    [Id]           NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BidderId]     NUMERIC (18)    NOT NULL,
    [PaymentMode]  SMALLINT        NOT NULL,
    [ChequeDDNo]   VARCHAR (20)    NULL,
    [ChequeDDDate] DATETIME        NULL,
    [PayableAt]    VARCHAR (50)    NOT NULL,
    [BankName]     VARCHAR (50)    NULL,
    [Amount]       DECIMAL (18, 2) NOT NULL,
    [Comments]     VARCHAR (250)   NULL,
    [ReceiptNo]    NUMERIC (18)    NULL,
    [EntryDate]    DATETIME        NOT NULL,
    [UpdatedOn]    DATETIME        NULL,
    [UpdatedBy]    NUMERIC (18)    NULL,
    CONSTRAINT [PK_AE_PaymentCredits] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

