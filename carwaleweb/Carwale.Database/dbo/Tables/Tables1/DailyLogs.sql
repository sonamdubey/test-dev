CREATE TABLE [dbo].[DailyLogs] (
    [ID]                        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EntryDate]                 DATETIME     NOT NULL,
    [UserRegistration]          NUMERIC (18) CONSTRAINT [DF_DailyLogs_UserRegistration] DEFAULT (0) NULL,
    [UniqueVisitors]            NUMERIC (18) CONSTRAINT [DF_DailyLogs_UniqueVisitors] DEFAULT (0) NULL,
    [Hits]                      NUMERIC (18) CONSTRAINT [DF_DailyLogs_Hits] DEFAULT (0) NULL,
    [NewPurchaseInq]            NUMERIC (18) CONSTRAINT [DF_DailyLogs_NewPurchaseInq] DEFAULT (0) NULL,
    [UsedPurchaseInqTotal]      NUMERIC (18) CONSTRAINT [DF_DailyLogs_UsedPurchaseInq] DEFAULT (0) NULL,
    [FinancePurchaseInq]        NUMERIC (18) CONSTRAINT [DF_DailyLogs_FinancePurchaseInq] DEFAULT (0) NULL,
    [SellInqTotal]              NUMERIC (18) CONSTRAINT [DF_DailyLogs_SellInq] DEFAULT (0) NULL,
    [DealerRegistration]        NUMERIC (18) CONSTRAINT [DF_DailyLogs_DealerRegistration] DEFAULT (0) NULL,
    [IndividualcarsSellInq]     NUMERIC (18) NULL,
    [DealerscarsSellInq]        NUMERIC (18) NULL,
    [UsedPurchaseInqIndividual] NUMERIC (18) NULL,
    [UsedPurchaseInqDealer]     NUMERIC (18) NULL,
    [PageViews]                 NUMERIC (18) NULL,
    CONSTRAINT [PK_DailyLogs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_DailyLogs] UNIQUE NONCLUSTERED ([EntryDate] ASC) WITH (FILLFACTOR = 90)
);

