CREATE TABLE [dbo].[TC_CarTradeRefurb] (
    [TC_CarTradeRefurbId]            INT           IDENTITY (1, 1) NOT NULL,
    [TC_CarTradeCertificationDataId] INT           NULL,
    [Category]                       VARCHAR (50)  NULL,
    [SubCategory]                    VARCHAR (50)  NULL,
    [SubName]                        VARCHAR (50)  NULL,
    [RepairCost]                     INT           NULL,
    [RepairCostValue]                INT           NULL,
    [Remarks]                        VARCHAR (100) NULL,
    CONSTRAINT [PK_TC_CarTradeRefurb] PRIMARY KEY CLUSTERED ([TC_CarTradeRefurbId] ASC)
);

