CREATE TABLE [dbo].[PriceQuoteJan2011] (
    [CWCustomerID]                      NUMERIC (18) NOT NULL,
    [CRMCustomerID]                     NUMERIC (18) NOT NULL,
    [LeadId]                            NUMERIC (18) NOT NULL,
    [PQId]                              NUMERIC (18) NOT NULL,
    [Make]                              VARCHAR (30) NOT NULL,
    [Model]                             VARCHAR (30) NOT NULL,
    [Version]                           VARCHAR (50) NULL,
    [ExShowroomPrice]                   NUMERIC (18) NOT NULL,
    [InsurancePrice]                    NUMERIC (18) NOT NULL,
    [RTOPrice]                          NUMERIC (18) NOT NULL,
    [LeadCreatedTime]                   DATETIME     NOT NULL,
    [PriceQuoteDate]                    DATETIME     NOT NULL,
    [CustomerEnteredExpectedBuyingDate] DATETIME     NOT NULL,
    [AgentConfirmedExpectedBuyingDate]  DATETIME     NULL,
    [ActualBuyingDate]                  DATETIME     NULL
);

