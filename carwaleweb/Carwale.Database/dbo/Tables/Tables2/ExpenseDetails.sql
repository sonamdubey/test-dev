CREATE TABLE [dbo].[ExpenseDetails] (
    [Id]             NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VoucherId]      VARCHAR (50)    NOT NULL,
    [ExpenseDetails] VARCHAR (100)   NOT NULL,
    [Head]           VARCHAR (100)   NULL,
    [ExpenseDate]    DATETIME        NULL,
    [Amount]         DECIMAL (18, 2) NOT NULL,
    [isDeleted]      BIT             CONSTRAINT [DF_ExpenseDetails_isDelete] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_ExpenseDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

