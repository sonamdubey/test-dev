﻿CREATE TYPE [dbo].[DCRM_TblPaymentDetails] AS TABLE (
    [TransactionId]      INT             NULL,
    [PaymentType]        SMALLINT        NULL,
    [ProductAmount]      NUMERIC (18, 2) NULL,
    [ServiceTax]         FLOAT (53)      NULL,
    [IsTDSGiven]         BIT             NULL,
    [TDSAmount]          NUMERIC (18, 2) NULL,
    [PANNumber]          VARCHAR (10)    NULL,
    [TANNumber]          VARCHAR (10)    NULL,
    [FinalProductAmount] NUMERIC (18, 2) NULL,
    [Mode]               INT             NULL,
    [Amount]             FLOAT (53)      NULL,
    [CheckNumber]        VARCHAR (50)    NULL,
    [AttachedFile]       VARCHAR (500)   NULL,
    [FileHostUrl]        VARCHAR (100)   NULL,
    [BankName]           VARCHAR (100)   NULL,
    [BranchName]         VARCHAR (100)   NULL,
    [DrawerName]         VARCHAR (100)   NULL,
    [InFavorOf]          VARCHAR (100)   NULL,
    [DepositedBy]        VARCHAR (100)   NULL,
    [UtrTransactionId]   VARCHAR (50)    NULL,
    [PaymentDate]        DATETIME        NULL,
    [ChequeDDPdcDate]    DATETIME        NULL,
    [DepositedDate]      DATETIME        NULL,
    [IssuerBankName]     VARCHAR (300)   NULL,
    [UpdatedBy]          INT             NULL,
    [UpdatedOn]          DATETIME        NULL,
    [Comments]           VARCHAR (500)   NULL);

