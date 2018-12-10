CREATE TABLE [dbo].[CustomersVerified] (
    [CustomerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CustomersVerified] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

