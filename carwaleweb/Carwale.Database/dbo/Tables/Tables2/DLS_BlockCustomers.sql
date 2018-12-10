CREATE TABLE [dbo].[DLS_BlockCustomers] (
    [CustomerId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DLS_BlockCustomers] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

