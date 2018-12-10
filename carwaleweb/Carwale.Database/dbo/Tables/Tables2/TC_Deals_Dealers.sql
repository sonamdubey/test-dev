CREATE TABLE [dbo].[TC_Deals_Dealers] (
    [DealerId]           INT           NOT NULL,
    [EnteredOn]          DATETIME      NOT NULL,
    [EnteredBy]          INT           NOT NULL,
    [IsDealerDealActive] BIT           CONSTRAINT [DF_TC_Deals_Dealers_IsDealerDealActive] DEFAULT ((1)) NOT NULL,
    [ContactEmail]       VARCHAR (200) NOT NULL,
    [ContactMobile]      VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TC_Deals_Dealers] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

