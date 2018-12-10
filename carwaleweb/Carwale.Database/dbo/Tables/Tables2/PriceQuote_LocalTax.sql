CREATE TABLE [dbo].[PriceQuote_LocalTax] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [CityId]         INT           NOT NULL,
    [CategoryItemid] INT           NOT NULL,
    [Rate]           FLOAT (53)    NOT NULL,
    [Description]    VARCHAR (500) NULL,
    [IsTaxOnTax]     BIT           NULL,
    [IsMultiTax]     BIT           NULL,
    CONSTRAINT [PK_PriceQuote_LocalTax] PRIMARY KEY CLUSTERED ([Id] ASC)
);

