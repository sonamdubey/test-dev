CREATE TABLE [dbo].[OLM_OEMSales] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MakeId]    NUMERIC (18) NOT NULL,
    [ModelId]   NUMERIC (18) NULL,
    [MonthVal]  DATETIME     NOT NULL,
    [Sales]     NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     CONSTRAINT [DF_OLM_OEMSales_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_OEMSales] PRIMARY KEY CLUSTERED ([Id] ASC)
);

