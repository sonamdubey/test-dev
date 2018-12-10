CREATE TYPE [dbo].[DCRM_PaymentDetails] AS TABLE (
    [SalesDealerID] INT          NULL,
    [Mode]          INT          NULL,
    [Amount]        INT          NULL,
    [CheckNomber]   VARCHAR (50) NULL,
    [BankName]      VARCHAR (50) NULL,
    [PaymentDate]   DATETIME     NULL,
    [UpdatedBy]     INT          NULL,
    [UpdatedOn]     DATETIME     NULL);

