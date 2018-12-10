CREATE TABLE [dbo].[OLM_OEMModelSales] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]   NUMERIC (18) NOT NULL,
    [MonthVal]  DATETIME     NOT NULL,
    [Sales]     NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     CONSTRAINT [DF_OLM_OEMModelSales_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_OEMModelSales] PRIMARY KEY CLUSTERED ([Id] ASC)
);

