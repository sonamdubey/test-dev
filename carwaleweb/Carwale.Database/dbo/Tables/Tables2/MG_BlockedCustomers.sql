CREATE TABLE [dbo].[MG_BlockedCustomers] (
    [CustomerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_MG_BlockedCustomers] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

