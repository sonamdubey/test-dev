CREATE TABLE [dbo].[TC_BlockedCustomers] (
    [CustomerMobile] VARCHAR (10) NOT NULL,
    [BlockDate]      DATETIME     CONSTRAINT [DF_TC_BlockedCustomers_BlockDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TC_BlockedCustomers] PRIMARY KEY CLUSTERED ([CustomerMobile] ASC)
);

