CREATE TABLE [dbo].[OLM_AudiBE_PGTransactions] (
    [Id]                   NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TransactionId]        NUMERIC (18)  NULL,
    [CustomerId]           NUMERIC (18)  NULL,
    [ClientIP]             VARCHAR (150) NULL,
    [UserAgent]            VARCHAR (500) NULL,
    [Amount]               NUMERIC (18)  NULL,
    [PaymentMode]          INT           NULL,
    [PaymentType]          INT           NULL,
    [EntryDate]            DATETIME      NULL,
    [ResponseCode]         NUMERIC (18)  NULL,
    [ResponseMsg]          VARCHAR (500) NULL,
    [EPGTransactionId]     VARCHAR (100) NULL,
    [AuthId]               VARCHAR (100) NULL,
    [ProcessCompleted]     BIT           NULL,
    [TransactionCompleted] BIT           NULL,
    CONSTRAINT [PK_OLM_AudiBE_PGTransactions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

