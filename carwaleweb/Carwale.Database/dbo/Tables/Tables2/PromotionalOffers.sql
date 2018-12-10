CREATE TABLE [dbo].[PromotionalOffers] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConsumerType]      SMALLINT     NOT NULL,
    [ConsumerId]        NUMERIC (18) NOT NULL,
    [PackageId]         INT          NOT NULL,
    [OfferCode]         VARCHAR (50) NOT NULL,
    [OfferValidity]     DATETIME     NOT NULL,
    [DiscountAmount]    NUMERIC (18) NOT NULL,
    [CreatedOn]         DATETIME     NOT NULL,
    [UsedOn]            DATETIME     NULL,
    [IsOfferUsed]       BIT          CONSTRAINT [DF_DealerOffers_IsOfferUsed] DEFAULT (0) NOT NULL,
    [IsBlocked]         BIT          CONSTRAINT [DF_DealerOffers_IsBlocked] DEFAULT (0) NOT NULL,
    [NoOfTrials]        INT          NULL,
    [OfferValidityFrom] DATETIME     NULL,
    [OfferValidityTo]   DATETIME     NULL,
    [IsArchived]        BIT          CONSTRAINT [DF_PromotionalOffers_IsArchived] DEFAULT ((0)) NULL,
    [LastUpdatedOn]     DATETIME     NULL,
    CONSTRAINT [PK_PromotionalOffers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

