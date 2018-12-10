CREATE TABLE [dbo].[Microsite_AtomPGTransactions] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [CustId]            INT           NULL,
    [DealerId]          INT           NULL,
    [Amount]            INT           NULL,
    [XmlResponse]       VARCHAR (MAX) NULL,
    [IPAddress]         VARCHAR (150) NULL,
    [UserAgent]         VARCHAR (500) NULL,
    [IsPaymentDone]     BIT           NULL,
    [IsPaymentSuccess]  BIT           NULL,
    [EntryDate]         DATETIME      DEFAULT (getdate()) NULL,
    [ModifiedDate]      DATETIME      NULL,
    [TransactionTypeId] INT           NULL,
    [PGFormResponse]    VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

