CREATE TABLE [dbo].[TC_PQRequestEMILog] (
    [TC_PQRequestEMILogId]    INT        IDENTITY (1, 1) NOT NULL,
    [TC_PQRequestEMIId]       INT        NULL,
    [TC_PriceQuoteRequestsId] INT        NULL,
    [Tenure]                  INT        NULL,
    [LoanAmount]              FLOAT (53) NULL,
    [RateOfInterest]          FLOAT (53) NULL,
    [DownPayment]             FLOAT (53) NULL,
    [EMIAmount]               INT        NULL,
    [EntryDate]               DATETIME   NULL,
    [EMICalculatedOn]         TINYINT    NULL
);

