CREATE TABLE [dbo].[ExpenseVoucher] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VoucherId]    VARCHAR (50)  NULL,
    [ExpenseOwner] VARCHAR (100) NOT NULL,
    [Place]        VARCHAR (50)  NOT NULL,
    [VoucherDate]  DATETIME      NOT NULL,
    [ApprovedBy]   VARCHAR (50)  NULL,
    [ReceivedBy]   VARCHAR (50)  NULL,
    [PreparedBy]   VARCHAR (50)  NULL,
    [IsCheque]     BIT           NULL,
    [isActive]     BIT           CONSTRAINT [DF_ExpenseBoucher_isActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ExpenseBoucher] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

