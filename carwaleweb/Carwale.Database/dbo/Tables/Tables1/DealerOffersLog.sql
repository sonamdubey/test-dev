﻿CREATE TABLE [dbo].[DealerOffersLog] (
    [ID]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [OfferId]            NUMERIC (18)  NULL,
    [OfferTitle]         VARCHAR (200) NULL,
    [OfferDescription]   VARCHAR (MAX) NULL,
    [MaxOfferValue]      NUMERIC (18)  NULL,
    [Conditions]         VARCHAR (MAX) NULL,
    [StartDate]          DATETIME      NULL,
    [EndDate]            DATETIME      NULL,
    [EntryDate]          DATETIME      NULL,
    [EnteredBy]          NUMERIC (18)  NULL,
    [SourceCategory]     NUMERIC (18)  NULL,
    [SourceDescription]  VARCHAR (200) NULL,
    [IsActive]           BIT           NULL,
    [IsApproved]         BIT           NULL,
    [IsCountryWide]      BIT           NULL,
    [OfferType]          SMALLINT      NULL,
    [OfferUnits]         INT           NULL,
    [ClaimedUnits]       INT           NULL,
    [UpdatedOn]          DATETIME      NULL,
    [UpdatedBy]          INT           NULL,
    [HostURL]            VARCHAR (250) NULL,
    [ImageName]          VARCHAR (150) NULL,
    [ImagePath]          VARCHAR (150) NULL,
    [PreBookingEmailIds] VARCHAR (200) NULL,
    [PreBookingMobile]   VARCHAR (200) NULL,
    [CouponEmailIds]     VARCHAR (200) NULL,
    [CouponMobile]       VARCHAR (200) NULL,
    [Remarks]            VARCHAR (100) NULL,
    [DispOnDesk]         BIT           CONSTRAINT [DF_DealerOffersLog_DispOnDesk] DEFAULT ((0)) NULL,
    [DispOnMobile]       BIT           CONSTRAINT [DF_DealerOffersLog_DispOnMobile] DEFAULT ((0)) NULL,
    [DispSnippetOnDesk]  BIT           NULL,
    [DispSnippetOnMob]   BIT           NULL,
    [DispOnOffersPgDesk] BIT           NULL,
    [DispOnOffersPgMob]  BIT           NULL,
    [ShortDescription]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_DealerOffersLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

