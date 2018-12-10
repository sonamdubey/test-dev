CREATE TABLE [dbo].[TempCarResponse] (
    [ProfileId]    NUMERIC (18) NOT NULL,
    [SellerType]   INT          NOT NULL,
    [Seller]       VARCHAR (10) NOT NULL,
    [EntryDate]    DATETIME     NOT NULL,
    [ResponseTime] DATETIME     NOT NULL,
    [CustomerId]   NUMERIC (18) NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_TempCarResponse]
    ON [dbo].[TempCarResponse]([ProfileId] ASC);

