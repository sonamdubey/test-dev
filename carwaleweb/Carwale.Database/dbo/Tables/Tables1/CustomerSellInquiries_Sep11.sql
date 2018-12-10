CREATE TABLE [dbo].[CustomerSellInquiries_Sep11] (
    [ProfileId]  NUMERIC (18) NOT NULL,
    [CustomerId] NUMERIC (18) NOT NULL,
    [EntryDate]  DATETIME     NOT NULL,
    [City]       VARCHAR (50) NOT NULL,
    [State]      VARCHAR (30) NOT NULL,
    [Make]       VARCHAR (30) NOT NULL,
    [Model]      VARCHAR (30) NOT NULL,
    [Version]    VARCHAR (50) NULL,
    [MakeYear]   DATETIME     NOT NULL,
    [Kilometers] NUMERIC (18) NOT NULL,
    [Price]      DECIMAL (18) NOT NULL,
    [Seller]     VARCHAR (10) NOT NULL
);

