CREATE TABLE [dbo].[TataDealerQuotes] (
    [InquiryId]       NUMERIC (18)  NOT NULL,
    [QuoteId]         NUMERIC (18)  NOT NULL,
    [DealerId]        NUMERIC (18)  NULL,
    [DealerName]      VARCHAR (250) NULL,
    [DealerContactNo] VARCHAR (200) NULL,
    CONSTRAINT [PK_TataDealerQuotes] PRIMARY KEY CLUSTERED ([InquiryId] ASC) WITH (FILLFACTOR = 90)
);

