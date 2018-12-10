CREATE TABLE [dbo].[analyticsdealer_classified_3m1] (
    [id]              NUMERIC (18) NOT NULL,
    [dealerid]        NUMERIC (18) NOT NULL,
    [carversionid]    NUMERIC (18) NOT NULL,
    [entrydate]       DATETIME     NOT NULL,
    [SellInquiryId]   NUMERIC (18) NULL,
    [CustomerId]      NUMERIC (18) NULL,
    [RequestDateTime] DATETIME     NULL,
    [Entrytime]       VARCHAR (30) NULL
);

