﻿CREATE TABLE [dbo].[CrossListingDupListingCustomerSellInquiries] (
    [ID]                   NUMERIC (18)   NOT NULL,
    [CustomerId]           NUMERIC (18)   NOT NULL,
    [CityId]               INT            NULL,
    [CarVersionId]         NUMERIC (18)   NOT NULL,
    [CarRegNo]             VARCHAR (50)   NULL,
    [EntryDate]            DATETIME       NOT NULL,
    [Price]                DECIMAL (18)   NOT NULL,
    [MakeYear]             DATETIME       NOT NULL,
    [Kilometers]           NUMERIC (18)   NULL,
    [Color]                VARCHAR (100)  NULL,
    [ColorCode]            VARCHAR (6)    NULL,
    [Comments]             VARCHAR (500)  NULL,
    [IsArchived]           BIT            NULL,
    [IsApproved]           BIT            NOT NULL,
    [ForwardDealers]       BIT            NOT NULL,
    [ListInClassifieds]    BIT            NOT NULL,
    [IsFake]               BIT            NULL,
    [StatusId]             SMALLINT       NULL,
    [LastBidDate]          DATETIME       NULL,
    [ClassifiedExpiryDate] DATETIME       NULL,
    [ViewCount]            NUMERIC (18)   NULL,
    [PaidInqLeft]          INT            NOT NULL,
    [FreeInqLeft]          INT            NOT NULL,
    [PackageType]          SMALLINT       NOT NULL,
    [PackageExpiryDate]    DATETIME       NULL,
    [SourceId]             SMALLINT       NOT NULL,
    [IsVerified]           BIT            NULL,
    [CarRegState]          CHAR (4)       NULL,
    [PinCode]              INT            NULL,
    [Progress]             INT            NULL,
    [ReasonForSelling]     VARCHAR (500)  NULL,
    [PackageId]            INT            NULL,
    [Referrer]             NVARCHAR (100) NULL,
    [IPAddress]            VARCHAR (150)  NULL,
    [IsPremium]            BIT            NULL,
    [CustomerName]         VARCHAR (100)  NULL,
    [CustomerEmail]        VARCHAR (100)  NULL,
    [CustomerMobile]       VARCHAR (20)   NULL,
    [IsListingCompleted]   BIT            NULL,
    [CurrentStep]          TINYINT        NULL,
    [ListingCompletedDate] DATETIME       NULL,
    [PaymentMode]          TINYINT        NULL
);

