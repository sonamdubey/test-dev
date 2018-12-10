CREATE TABLE [dbo].[ClassifiedRequests2408] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CustomerId]      NUMERIC (18)  NOT NULL,
    [SellInquiryId]   NUMERIC (18)  NOT NULL,
    [Comments]        VARCHAR (500) NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [SourceId]        SMALLINT      NULL
);

