CREATE TABLE [dbo].[TC_SyndicationDealer] (
    [TC_SyndicationDealerId]  INT      IDENTITY (1, 1) NOT NULL,
    [TC_SyndicationWebsiteId] SMALLINT NOT NULL,
    [BranchId]                INT      NOT NULL,
    [IsActive]                BIT      CONSTRAINT [DF_TC_SyndicationDealer_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_SyndicationDealer] PRIMARY KEY CLUSTERED ([TC_SyndicationDealerId] ASC)
);

