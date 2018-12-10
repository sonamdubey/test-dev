CREATE TABLE [dbo].[NewCarQuoteCompare_CW] (
    [Id]               NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerQuoteId]    NUMERIC (18)    NOT NULL,
    [QuoteId]          NUMERIC (18)    NULL,
    [FaId]             INT             NULL,
    [OptionType]       TINYINT         NOT NULL,
    [ExShowroomPrice]  NUMERIC (18)    NOT NULL,
    [Insurance]        NUMERIC (18)    NOT NULL,
    [RTO]              NUMERIC (18)    NOT NULL,
    [Discount]         NUMERIC (18)    NOT NULL,
    [LTV]              DECIMAL (18, 2) NOT NULL,
    [LoanAmount]       NUMERIC (18)    NOT NULL,
    [LoanTenure]       INT             NOT NULL,
    [StampDuty]        NUMERIC (18)    NOT NULL,
    [ProcessingFees]   NUMERIC (18)    NOT NULL,
    [EMI]              NUMERIC (18)    NOT NULL,
    [InterestRate]     DECIMAL (18, 2) NOT NULL,
    [TotalDownPayment] NUMERIC (18)    NOT NULL,
    CONSTRAINT [PK_NewCarQuoteCompare_CW] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

