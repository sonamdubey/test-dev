CREATE TABLE [dbo].[PaidSellerScore] (
    [ProfileId]    VARCHAR (50) NOT NULL,
    [Inquiryid]    NUMERIC (18) NULL,
    [SellerType]   SMALLINT     NULL,
    [CityId]       NUMERIC (18) NULL,
    [DealerId]     INT          NULL,
    [Score]        FLOAT (53)   NULL,
    [SVScore]      FLOAT (53)   NULL,
    [NewScore]     FLOAT (53)   NULL,
    [packagetype]  INT          NULL,
    [CreatedOn]    DATETIME     DEFAULT (getdate()) NULL,
    [ValueOffered] FLOAT (53)   NULL,
    [CustCount]    INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PaidSellerScore_Inquiryid_SellerType]
    ON [dbo].[PaidSellerScore]([Inquiryid] ASC, [SellerType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaidSellerScore_DealerId]
    ON [dbo].[PaidSellerScore]([DealerId] ASC);

