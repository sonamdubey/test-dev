CREATE TABLE [dbo].[CustomerCredits] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AccountId]    NUMERIC (18)  NOT NULL,
    [Amount]       NUMERIC (18)  CONSTRAINT [DF_CustomerCredits_Amount] DEFAULT (0) NOT NULL,
    [ModeId]       NUMERIC (18)  CONSTRAINT [DF_CustomerCredits_ModeId] DEFAULT (1) NOT NULL,
    [EntryDate]    DATETIME      NOT NULL,
    [ReceiptNo]    NUMERIC (18)  NOT NULL,
    [ReceiptDate]  DATETIME      NOT NULL,
    [BankName]     VARCHAR (50)  NULL,
    [ChequeNo]     VARCHAR (50)  NULL,
    [Description]  VARCHAR (300) NULL,
    [Encashed]     BIT           CONSTRAINT [DF_CustomerCredits_Encashed] DEFAULT (1) NOT NULL,
    [EncashedDate] DATETIME      CONSTRAINT [DF_CustomerCredits_IsActive] DEFAULT (1) NULL,
    [RecievedBy]   NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_CustomerCredits] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CustomerCredits_AccountCustomers] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[AccountCustomers] ([Id])
);

