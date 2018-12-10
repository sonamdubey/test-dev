CREATE TABLE [dbo].[CustomerDebits] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AccountId]       NUMERIC (18)  NOT NULL,
    [Amount]          NUMERIC (18)  CONSTRAINT [DF_CustomerDebits_Amount] DEFAULT (0) NOT NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [DebitCategoryId] NUMERIC (18)  NOT NULL,
    [Description]     VARCHAR (300) NULL,
    [GivenBy]         NUMERIC (18)  NOT NULL,
    [ModeId]          NUMERIC (18)  NOT NULL,
    [BankName]        VARCHAR (50)  NULL,
    [ChequeNo]        VARCHAR (25)  NULL,
    CONSTRAINT [PK_CustomerDebits] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CustomerDebits_AccountCustomers] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[AccountCustomers] ([Id]),
    CONSTRAINT [FK_CustomerDebits_DebitSubCategories] FOREIGN KEY ([DebitCategoryId]) REFERENCES [dbo].[DebitSubCategories] ([ID])
);

