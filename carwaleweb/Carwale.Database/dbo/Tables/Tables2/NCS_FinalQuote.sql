CREATE TABLE [dbo].[NCS_FinalQuote] (
    [Id]                    NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PQRefNo]               VARCHAR (50)    NULL,
    [FAId]                  INT             NOT NULL,
    [CustomerName]          VARCHAR (50)    NOT NULL,
    [Email]                 VARCHAR (100)   NOT NULL,
    [Mobile]                VARCHAR (12)    NOT NULL,
    [Comments]              VARCHAR (500)   NULL,
    [ExShowroomPrice]       INT             NULL,
    [FinanceAmount]         INT             NOT NULL,
    [MarginAmount]          INT             NOT NULL,
    [FinanceTenure]         INT             NOT NULL,
    [FinanceOption]         TINYINT         NOT NULL,
    [EMI]                   INT             NOT NULL,
    [StampDuty]             INT             NOT NULL,
    [ProcessingFees]        INT             NOT NULL,
    [CarwaleDiscount]       INT             NOT NULL,
    [InsuranceAmount]       INT             NOT NULL,
    [RTO]                   INT             NOT NULL,
    [OtherChargesName]      VARCHAR (50)    NOT NULL,
    [OtherCharges]          INT             NOT NULL,
    [TotalDownPayment]      INT             NOT NULL,
    [EffectiveInterestRate] DECIMAL (18, 2) NOT NULL,
    [CwDealerCommission]    DECIMAL (18)    NULL,
    [Status]                BIT             CONSTRAINT [DF_NCS_FinalQuote_Status] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_NCS_FinalQuote] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for "Low down payment plan" and 2 for  "Low EMI plan"', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_FinalQuote';

