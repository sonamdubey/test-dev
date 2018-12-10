CREATE TABLE [dbo].[TC_PurhaseInq29Feb] (
    [DealerId]        NUMERIC (18)  NULL,
    [TC_StockId]      NUMERIC (18)  NULL,
    [Name]            VARCHAR (100) NULL,
    [email]           VARCHAR (100) NULL,
    [Mobile]          VARCHAR (20)  NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    [sourceid]        INT           NOT NULL,
    [inquirystatusid] INT           NOT NULL,
    [TC_CustomerId]   BIGINT        NULL
);

