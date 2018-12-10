CREATE TABLE [dbo].[NewCarQuoteCompare_Dealer] (
    [Id]                     NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]              INT          NOT NULL,
    [CityId]                 INT          NOT NULL,
    [ExShowroomPrice]        NUMERIC (18) NOT NULL,
    [RTO]                    INT          NOT NULL,
    [Insurance]              INT          NOT NULL,
    [OEMDiscounts]           INT          NULL,
    [ExchangeBonus]          INT          NULL,
    [CorporateDiscount]      INT          NULL,
    [InsuranceDiscount]      INT          NULL,
    [OtherDiscounts]         INT          NULL,
    [TotalDiscount]          INT          NULL,
    [LoanAmount]             NUMERIC (18) NOT NULL,
    [LoanTenure]             SMALLINT     NOT NULL,
    [EMI]                    INT          NOT NULL,
    [TotalDownPayment]       NUMERIC (18) NOT NULL,
    [StampDutyProcessingFee] INT          NULL,
    CONSTRAINT [PK_NewCarQuoteCompare_Dealer] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

