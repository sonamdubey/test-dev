CREATE TABLE [dbo].[MapNewCarInqFinance] (
    [NewCarInquiryId] NUMERIC (18) NOT NULL,
    [LoanId]          NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_MapNewCarInqFinance] PRIMARY KEY CLUSTERED ([NewCarInquiryId] ASC, [LoanId] ASC) WITH (FILLFACTOR = 90)
);

