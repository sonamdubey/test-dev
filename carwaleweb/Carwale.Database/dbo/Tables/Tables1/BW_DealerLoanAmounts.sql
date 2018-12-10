CREATE TABLE [dbo].[BW_DealerLoanAmounts] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]          INT           NOT NULL,
    [Tenure]            TINYINT       NULL,
    [RateOfInterest]    FLOAT (53)    NULL,
    [LTV]               TINYINT       NULL,
    [LoanProvider]      VARCHAR (100) NULL,
    [MinDownPayment]    FLOAT (53)    NULL,
    [MaxDownPayment]    FLOAT (53)    NULL,
    [MinTenure]         INT           NULL,
    [MaxTenure]         INT           NULL,
    [MinRateOfInterest] FLOAT (53)    NULL,
    [MaxRateOfInterest] FLOAT (53)    NULL,
    [ProcessingFee]     FLOAT (53)    NULL,
    [UserId]            INT           NULL,
    [IsActive]          BIT           DEFAULT ((1)) NULL,
    [MinLtv]            FLOAT (53)    NULL,
    [MaxLtv]            FLOAT (53)    NULL
);

