CREATE TABLE [dbo].[CustomersBlockedMobiles] (
    [MobileNo] VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_CustomersBlockedMobiles] PRIMARY KEY CLUSTERED ([MobileNo] ASC) WITH (FILLFACTOR = 90)
);

