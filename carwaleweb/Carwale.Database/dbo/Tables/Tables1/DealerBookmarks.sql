CREATE TABLE [dbo].[DealerBookmarks] (
    [DealerId]      NUMERIC (18)  NOT NULL,
    [SellInquiryId] NUMERIC (18)  NOT NULL,
    [comments]      VARCHAR (500) NULL,
    CONSTRAINT [PK_DealerBookmarks] PRIMARY KEY CLUSTERED ([DealerId] ASC, [SellInquiryId] ASC) WITH (FILLFACTOR = 90)
);

