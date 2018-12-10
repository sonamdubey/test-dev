CREATE TABLE [dbo].[OnlineInsuranceLeads] (
    [MyCarwaleCarId]  NUMERIC (18) NOT NULL,
    [IsNew]           BIT          NOT NULL,
    [ClientIPAddress] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OnlineInsuranceLeads] PRIMARY KEY CLUSTERED ([MyCarwaleCarId] ASC) WITH (FILLFACTOR = 90)
);

