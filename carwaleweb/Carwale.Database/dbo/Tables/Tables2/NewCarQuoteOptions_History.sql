CREATE TABLE [dbo].[NewCarQuoteOptions_History] (
    [Id]                    NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuoteId]               NUMERIC (18)    NOT NULL,
    [FinanceOption]         TINYINT         NOT NULL,
    [FaId]                  INT             NOT NULL,
    [ExShowroomPrice]       INT             NOT NULL,
    [FinanceAmount]         INT             NOT NULL,
    [MarginAmount]          INT             NOT NULL,
    [Tenure]                INT             NOT NULL,
    [LTV]                   DECIMAL (18)    NULL,
    [EMI]                   INT             NOT NULL,
    [StampDuty]             INT             NOT NULL,
    [ProcessingFees]        INT             NOT NULL,
    [CwDiscount]            INT             NOT NULL,
    [OtherChargesName]      VARCHAR (50)    NOT NULL,
    [OtherCharges]          DECIMAL (18, 2) NOT NULL,
    [TotalDownPayment]      INT             NOT NULL,
    [InterestRateType]      SMALLINT        NOT NULL,
    [EffectiveInterestRate] DECIMAL (18, 2) NOT NULL,
    [CwDealerCommission]    DECIMAL (18)    NULL,
    [EntryDateTime]         DATETIME        NOT NULL,
    CONSTRAINT [PK_NewCarQuoteOptions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

