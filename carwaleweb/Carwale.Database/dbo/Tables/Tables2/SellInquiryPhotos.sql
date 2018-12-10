CREATE TABLE [dbo].[SellInquiryPhotos] (
    [SellInquiryId] DECIMAL (18) NOT NULL,
    [PhotoUrl]      VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_SellInquiryPhotos] PRIMARY KEY CLUSTERED ([SellInquiryId] ASC) WITH (FILLFACTOR = 90)
);

