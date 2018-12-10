CREATE TABLE [dbo].[TC_BookingFinance] (
    [TC_BookingFinance_Id] INT      IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]     INT      NOT NULL,
    [TC_DealerBank_Id]     INT      NULL,
    [IsDocumentReceived]   BIT      CONSTRAINT [DF_TC_BookingFinance_IsDocumentReceived] DEFAULT ((0)) NULL,
    [IsCaseLoggedIn]       BIT      CONSTRAINT [DF_TC_BookingFinance_IsCaseLoggedIn] DEFAULT ((0)) NULL,
    [IsLoanApproved]       BIT      CONSTRAINT [DF_TC_BookingFinance_IsLoanApproved] DEFAULT ((0)) NULL,
    [LoanApprovalDate]     DATE     NULL,
    [AmountApproved]       INT      NULL,
    [LoanTerms]            TINYINT  NULL,
    [IsDisbursed]          BIT      CONSTRAINT [DF_TC_BookingFinance_IsDisbursed] DEFAULT ((0)) NULL,
    [DisbursedDate]        DATE     NULL,
    [EntryDate]            DATETIME CONSTRAINT [DF_TC_BookingFinance_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsActive]             BIT      CONSTRAINT [DF_TC_BookingFinance_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]         DATE     NULL,
    [ModifiedBy]           INT      NULL,
    CONSTRAINT [PK_TC_BookingFinance] PRIMARY KEY CLUSTERED ([TC_BookingFinance_Id] ASC),
    CONSTRAINT [UNQ_CarBookingId] UNIQUE NONCLUSTERED ([TC_CarBooking_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the Bank for the finance,come from the master entry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_BookingFinance', @level2type = N'COLUMN', @level2name = N'TC_DealerBank_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'When this row is entered', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_BookingFinance', @level2type = N'COLUMN', @level2name = N'EntryDate';

