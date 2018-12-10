CREATE TABLE [dbo].[NewCarQuoteChoosen] (
    [PQId]   NUMERIC (18) NOT NULL,
    [LoanId] NUMERIC (18) NOT NULL,
    [FAId]   INT          NOT NULL,
    CONSTRAINT [PK_NewCarQuoteChoosen] PRIMARY KEY CLUSTERED ([PQId] ASC, [LoanId] ASC, [FAId] ASC) WITH (FILLFACTOR = 90)
);

