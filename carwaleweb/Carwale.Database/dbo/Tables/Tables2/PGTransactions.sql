CREATE TABLE [dbo].[PGTransactions] (
    [ID]                     NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerType]           SMALLINT       NOT NULL,
    [ConsumerId]             NUMERIC (18)   NOT NULL,
    [CarId]                  NUMERIC (18)   NULL,
    [PackageId]              NUMERIC (18)   NOT NULL,
    [Amount]                 NUMERIC (18)   NOT NULL,
    [PackageDesc]            VARCHAR (1500) NULL,
    [EntryDateTime]          DATETIME       NOT NULL,
    [ResponseCode]           NUMERIC (18)   NULL,
    [ResponseMessage]        VARCHAR (500)  NULL,
    [EPGTransactionId]       VARCHAR (100)  NULL,
    [AuthId]                 VARCHAR (100)  NULL,
    [ProcessCompleted]       BIT            CONSTRAINT [DF_PGTransactions_ProcessCompleted] DEFAULT (0) NOT NULL,
    [TransactionCompleted]   BIT            CONSTRAINT [DF_PGTransactions_TransactionCompleted] DEFAULT (0) NOT NULL,
    [IPAddress]              VARCHAR (150)  NULL,
    [UserAgent]              VARCHAR (500)  NULL,
    [PGSource]               SMALLINT       CONSTRAINT [DF_PGTransactions_PGSource] DEFAULT (1) NOT NULL,
    [PlatformId]             SMALLINT       NULL,
    [ApplicationId]          SMALLINT       NULL,
    [TransactionReferenceId] VARCHAR (20)   NULL,
    CONSTRAINT [PK_PGTransactions] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_PGTransactions__PackageId__EntryDateTime]
    ON [dbo].[PGTransactions]([PackageId] ASC, [EntryDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_PGTransactions__ConsumerType__ResponseCode__EntryDateTime]
    ON [dbo].[PGTransactions]([ConsumerType] ASC, [ResponseCode] ASC, [EntryDateTime] ASC)
    INCLUDE([ConsumerId], [PackageId]);


GO
CREATE NONCLUSTERED INDEX [IX_PGTransactions_CarId]
    ON [dbo].[PGTransactions]([CarId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for ICICI, 2 for CCAvenue', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PGTransactions', @level2type = N'COLUMN', @level2name = N'PGSource';

