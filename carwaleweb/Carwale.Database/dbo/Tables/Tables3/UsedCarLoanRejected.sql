CREATE TABLE [dbo].[UsedCarLoanRejected] (
    [LoanId]     NUMERIC (18)  NOT NULL,
    [Reason]     VARCHAR (500) NULL,
    [LeadDate]   DATETIME      NULL,
    [RejectDate] DATETIME      CONSTRAINT [DF_UsedCarLoadRejected_RejectDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UsedCarLoadRejected] PRIMARY KEY CLUSTERED ([LoanId] ASC) WITH (FILLFACTOR = 90)
);

