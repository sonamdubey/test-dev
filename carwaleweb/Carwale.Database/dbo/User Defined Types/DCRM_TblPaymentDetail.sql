CREATE TYPE [dbo].[DCRM_TblPaymentDetail] AS TABLE (
    [TransactionId]    INT           NULL,
    [PaymentType]      SMALLINT      NULL,
    [Mode]             INT           NULL,
    [Amount]           FLOAT (53)    NULL,
    [CheckNumber]      VARCHAR (50)  NULL,
    [BankName]         VARCHAR (100) NULL,
    [BranchName]       VARCHAR (100) NULL,
    [DrawerName]       VARCHAR (100) NULL,
    [InFavorOf]        VARCHAR (100) NULL,
    [DepositedBy]      VARCHAR (100) NULL,
    [UtrTransactionId] VARCHAR (20)  NULL,
    [PaymentDate]      DATETIME      NULL,
    [ChequeDDPdcDate]  DATETIME      NULL,
    [DepositedDate]    DATETIME      NULL,
    [UpdatedBy]        INT           NULL,
    [UpdatedOn]        DATETIME      NULL);

