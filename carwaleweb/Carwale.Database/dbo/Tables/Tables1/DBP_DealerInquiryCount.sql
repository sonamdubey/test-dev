CREATE TABLE [dbo].[DBP_DealerInquiryCount] (
    [DealerId]     NUMERIC (18) NOT NULL,
    [InquiryCount] NUMERIC (18) NULL,
    CONSTRAINT [PK_DBP_DealerInquiryCount] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90)
);

