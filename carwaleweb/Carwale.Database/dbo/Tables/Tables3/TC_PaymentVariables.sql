CREATE TABLE [dbo].[TC_PaymentVariables] (
    [TC_PaymentVariables_Id] INT          IDENTITY (1, 1) NOT NULL,
    [VariableName]           VARCHAR (20) NOT NULL,
    [IsActive]               BIT          CONSTRAINT [DF_TC_PaymentVariables_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]              DATETIME     DEFAULT (getdate()) NULL,
    [ModifiedBy]             INT          NULL,
    [ModifiedDate]           DATETIME     NULL,
    CONSTRAINT [PK_TC_PaymentVariables] PRIMARY KEY CLUSTERED ([TC_PaymentVariables_Id] ASC)
);

