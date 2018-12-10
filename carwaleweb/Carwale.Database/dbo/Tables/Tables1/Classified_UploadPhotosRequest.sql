CREATE TABLE [dbo].[Classified_UploadPhotosRequest] (
    [SellInquiryId]   NCHAR (10)    NOT NULL,
    [BuyerId]         NUMERIC (18)  NOT NULL,
    [ConsumerType]    TINYINT       CONSTRAINT [DF_Classified_UploadPhotosRequest_ConsumerType] DEFAULT ((1)) NOT NULL,
    [BuyerMessage]    VARCHAR (200) NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_Classified_UploadPhotosRequest] PRIMARY KEY CLUSTERED ([SellInquiryId] ASC, [BuyerId] ASC, [ConsumerType] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Classified_UploadPhotosRequest__BuyerId__ConsumerType]
    ON [dbo].[Classified_UploadPhotosRequest]([BuyerId] ASC, [ConsumerType] ASC);

