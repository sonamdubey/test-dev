CREATE TABLE [dbo].[DealerOfferLeads] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [OfferId]         NUMERIC (18) NOT NULL,
    [CustomerId]      NUMERIC (18) NOT NULL,
    [BuyTime]         VARCHAR (50) NULL,
    [RequestDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_DealerOfferLeads] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

