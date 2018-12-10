CREATE TABLE [dbo].[CustomerSecurityKey] (
    [CustomerId]  NUMERIC (18) NOT NULL,
    [CustomerKey] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CustomerSecurityKey] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_CustomerSecurityKey] UNIQUE NONCLUSTERED ([CustomerKey] ASC) WITH (FILLFACTOR = 90)
);

