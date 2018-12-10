CREATE TABLE [dbo].[Microsite_DealerOffers] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]            INT           NOT NULL,
    [OfferTitle]          VARCHAR (300) NULL,
    [OfferDetails]        VARCHAR (MAX) NULL,
    [TillStockLast]       BIT           CONSTRAINT [DF_Microsite_DealerOffers_TillStockLast] DEFAULT ((0)) NOT NULL,
    [OfferStartDate]      DATE          NULL,
    [OfferEndDate]        DATE          NULL,
    [OfferTermsCondition] VARCHAR (MAX) NULL,
    [IsActive]            BIT           CONSTRAINT [DF_NCD_Offers_IsActive] DEFAULT ((1)) NOT NULL,
    [OfferContent]        VARCHAR (MAX) NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_Microsite_DealerOffers_IsDeleted] DEFAULT ((0)) NULL,
    [CityId]              INT           NULL,
    [ModelId]             INT           NULL,
    [OfferOrder]          INT           NULL,
    [IsFeatured]          BIT           NULL,
    [HostUrl]             VARCHAR (50)  NULL,
    [OriginalImgPath]     VARCHAR (250) NULL,
    CONSTRAINT [PK_NCD_Offers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

