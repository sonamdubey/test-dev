CREATE TABLE [DCRM].[DealerPlanPrintLog] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]    NUMERIC (18)  NOT NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_DealerPlanPrintLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   NUMERIC (18)  NOT NULL,
    [ProductType] NUMERIC (18)  NULL,
    [ExpiryDate]  DATETIME      NULL,
    [CourierNo]   VARCHAR (100) NULL,
    [CourierDate] DATETIME      NULL,
    CONSTRAINT [PK_DealerPlanPrintLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

