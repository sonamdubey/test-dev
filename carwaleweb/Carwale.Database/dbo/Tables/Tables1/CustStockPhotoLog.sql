CREATE TABLE [dbo].[CustStockPhotoLog] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [InquiryId] INT      NOT NULL,
    [EntryDate] DATETIME NOT NULL,
    CONSTRAINT [PK_CustStockPhotoLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

