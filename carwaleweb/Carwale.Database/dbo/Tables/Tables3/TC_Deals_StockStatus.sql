CREATE TABLE [dbo].[TC_Deals_StockStatus] (
    [Id]            TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [IsActive]      BIT          NOT NULL,
    [IsDealer]      BIT          NULL,
    [IsCWExecutive] BIT          NULL,
    [IsVisible]     BIT          NULL,
    [Description]   NCHAR (50)   NULL,
    CONSTRAINT [PK_TC_Deals_StockStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

