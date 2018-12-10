CREATE TABLE [dbo].[ConsumersSoldPrice] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SellInquiryId] NUMERIC (18) NOT NULL,
    [UserType]      SMALLINT     NOT NULL,
    [SoldAtPrice]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ConsumersSoldPrice] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

