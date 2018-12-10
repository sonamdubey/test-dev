CREATE TABLE [dbo].[CDTransactions] (
    [ID]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerType]  SMALLINT       NOT NULL,
    [ConsumerId]    NUMERIC (18)   NOT NULL,
    [CarId]         NUMERIC (18)   NULL,
    [PackageId]     NUMERIC (18)   NOT NULL,
    [Amount]        NUMERIC (18)   NOT NULL,
    [PackageDesc]   VARCHAR (1500) NULL,
    [EntryDateTime] DATETIME       NOT NULL,
    [IPAddress]     VARCHAR (150)  NULL,
    [UserAgent]     VARCHAR (500)  NULL,
    [IsActive]      BIT            CONSTRAINT [DF_CDTransactions_IsActive] DEFAULT (1) NOT NULL,
    [ChequeNumber]  VARCHAR (100)  NULL,
    [BankName]      VARCHAR (500)  NULL,
    [BranchName]    VARCHAR (150)  NULL,
    [BankCity]      VARCHAR (150)  NULL,
    CONSTRAINT [PK_CDTransactions] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

