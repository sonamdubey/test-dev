CREATE TABLE [dbo].[MG_ShippingAddress] (
    [CustomerId]      NUMERIC (18)  NOT NULL,
    [ShippingAddress] VARCHAR (200) NOT NULL,
    [PinCode]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_MG_ShippingAddress] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

