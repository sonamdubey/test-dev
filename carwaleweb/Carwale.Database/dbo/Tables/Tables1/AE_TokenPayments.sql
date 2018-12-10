CREATE TABLE [dbo].[AE_TokenPayments] (
    [Id]               NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BidderId]         NUMERIC (18)    NOT NULL,
    [PaymentMode]      SMALLINT        NOT NULL,
    [ChequeDDNo]       VARCHAR (20)    NULL,
    [ChequeDDDate]     DATETIME        NULL,
    [PayableAt]        VARCHAR (50)    NULL,
    [BankName]         VARCHAR (50)    NULL,
    [Amount]           DECIMAL (18, 2) NOT NULL,
    [NoOfTokens]       INT             NOT NULL,
    [Status]           SMALLINT        NOT NULL,
    [EntryDate]        DATETIME        NOT NULL,
    [StatusActionDate] DATETIME        NULL,
    [UpdatedOn]        DATETIME        NULL,
    [UpdatedBy]        NUMERIC (18)    NULL,
    [RequestId]        NUMERIC (18)    NULL,
    CONSTRAINT [PK_AE_TokenPayments] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 - Nothing, DD Available, 1 - Encashed, 2 - Refund', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AE_TokenPayments', @level2type = N'COLUMN', @level2name = N'Status';

