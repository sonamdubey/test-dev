﻿CREATE TABLE [dbo].[SkodaBookings] (
    [Id]                      NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BookingCode]             VARCHAR (50)  NULL,
    [CarId]                   INT           NOT NULL,
    [CustomerId]              NUMERIC (18)  NULL,
    [DealerId]                INT           NULL,
    [ColorId]                 NUMERIC (18)  NULL,
    [CustomerName]            VARCHAR (100) NULL,
    [CustomerEmail]           VARCHAR (100) NULL,
    [CustomerMobile]          VARCHAR (12)  NULL,
    [CustomerCity]            INT           NULL,
    [IsBooked]                BIT           CONSTRAINT [DF_SkodaBookings_IsBooked] DEFAULT ((0)) NULL,
    [BookingAmount]           INT           NULL,
    [ExShowroom]              INT           NULL,
    [RTO]                     INT           NULL,
    [Insurance]               INT           NULL,
    [DealerDiscount]          INT           NULL,
    [PaymentMode]             SMALLINT      NULL,
    [PaymentDetails]          VARCHAR (50)  NULL,
    [IsPaymentRealised]       BIT           CONSTRAINT [DF_SkodaBookings_IsPaymentRealised] DEFAULT ((0)) NULL,
    [PaymentRealisedDateTime] DATETIME      NULL,
    [IsRefunded]              BIT           CONSTRAINT [DF_SkodaBookings_IsRefunded] DEFAULT ((0)) NULL,
    [IsCancelled]             BIT           CONSTRAINT [DF_SkodaBookings_IsCancelled] DEFAULT ((0)) NULL,
    [EntryDateTime]           DATETIME      NOT NULL,
    CONSTRAINT [PK_SkodaBookings_1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

