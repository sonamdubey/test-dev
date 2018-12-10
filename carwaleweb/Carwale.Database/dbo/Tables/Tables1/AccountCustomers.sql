CREATE TABLE [dbo].[AccountCustomers] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustId]       NUMERIC (18)  NOT NULL,
    [CustTypeId]   NUMERIC (18)  NOT NULL,
    [CustomerName] VARCHAR (50)  NOT NULL,
    [Address]      VARCHAR (255) NULL,
    [City]         VARCHAR (50)  NULL,
    [Pincode]      VARCHAR (6)   NULL,
    [IsActive]     BIT           CONSTRAINT [DF_AccountCustomers_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_AccountCustomers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_AccountCustomers_CustomerType] FOREIGN KEY ([CustTypeId]) REFERENCES [dbo].[CustomerType] ([Id])
);

