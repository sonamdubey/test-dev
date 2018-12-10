CREATE TABLE [dbo].[RSAOfferClaim] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [CustomerName]       VARCHAR (100) NULL,
    [CustomerMobile]     VARCHAR (50)  NULL,
    [CustomerEmail]      VARCHAR (100) NULL,
    [BikeRegistrationNo] VARCHAR (50)  NULL,
    [CustomerAddress]    VARCHAR (150) NULL,
    [DeliveryDate]       DATETIME      NULL,
    [DealerName]         VARCHAR (50)  NULL,
    [DealerAddress]      VARCHAR (150) NULL,
    [EntryDate]          DATETIME      NULL
);

