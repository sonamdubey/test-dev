CREATE TABLE [dbo].[DealerOffers_BackUp_30112014] (
    [ID]                NUMERIC (18)   NOT NULL,
    [CityId]            NUMERIC (18)   NULL,
    [DealerId]          NUMERIC (18)   NULL,
    [OfferTitle]        VARCHAR (200)  NULL,
    [OfferDescription]  VARCHAR (1000) NULL,
    [MaxOfferValue]     NUMERIC (18)   NULL,
    [Conditions]        VARCHAR (1000) NULL,
    [StartDate]         DATETIME       NULL,
    [EndDate]           DATETIME       NULL,
    [EntryDate]         DATETIME       NULL,
    [EnteredBy]         NUMERIC (18)   NULL,
    [SourceCategory]    NUMERIC (18)   NULL,
    [SourceDescription] VARCHAR (200)  NULL,
    [IsActive]          BIT            NULL,
    [IsApproved]        BIT            NOT NULL,
    [IsCountryWide]     BIT            NOT NULL
);

