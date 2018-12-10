CREATE TABLE [dbo].[PaidSellerScore_Bkp05012015] (
    [ProfileId]    VARCHAR (50) NOT NULL,
    [Inquiryid]    NUMERIC (18) NULL,
    [SellerType]   SMALLINT     NULL,
    [CityId]       NUMERIC (18) NULL,
    [DealerId]     INT          NULL,
    [Score]        FLOAT (53)   NULL,
    [SVScore]      FLOAT (53)   NULL,
    [NewScore]     FLOAT (53)   NULL,
    [packagetype]  INT          NULL,
    [CreatedOn]    DATETIME     NULL,
    [ValueOffered] FLOAT (53)   NULL
);

