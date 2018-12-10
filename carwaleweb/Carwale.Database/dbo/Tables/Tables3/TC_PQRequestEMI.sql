CREATE TABLE [dbo].[TC_PQRequestEMI] (
    [TC_PQRequestEMIId]       INT        IDENTITY (1, 1) NOT NULL,
    [TC_PriceQuoteRequestsId] INT        NULL,
    [Tenure]                  INT        NULL,
    [LoanAmount]              FLOAT (53) NULL,
    [RateOfInterest]          FLOAT (53) NULL,
    [DownPayment]             FLOAT (53) NULL,
    [EMIAmount]               INT        NULL,
    [EntryDate]               DATETIME   NULL,
    [EMICalculatedOn]         TINYINT    NULL
);

