CREATE TABLE [dbo].[TC_NewCarBooking] (
    [TC_NewCarBookingId]         INT           IDENTITY (1, 1) NOT NULL,
    [TC_NewCarInquiriesId]       INT           NULL,
    [RequestedDate]              DATETIME      CONSTRAINT [DF_TC_NewCarBooking_RequestedDate] DEFAULT (getdate()) NULL,
    [BookingStatus]              TINYINT       NULL,
    [Price]                      DECIMAL (18)  NULL,
    [Payment]                    DECIMAL (18)  NULL,
    [PendingPayment]             DECIMAL (18)  NULL,
    [IsLoanRequired]             BIT           NULL,
    [LastUpdatedDate]            DATETIME      CONSTRAINT [DF_TC_NewCarBooking_LastUpdatedDate] DEFAULT (getdate()) NULL,
    [BookingCompletedDate]       DATETIME      NULL,
    [TC_UsersId]                 INT           NULL,
    [VinNo]                      VARCHAR (50)  NULL,
    [PanNo]                      VARCHAR (25)  NULL,
    [BillingAddress]             VARCHAR (200) NULL,
    [CustomerName]               VARCHAR (100) NULL,
    [Pincode]                    VARCHAR (6)   NULL,
    [ContactNo]                  VARCHAR (15)  NULL,
    [Email]                      VARCHAR (100) NULL,
    [CorrespondenceAddress]      VARCHAR (200) NULL,
    [CorrespondenceCustomerName] VARCHAR (100) NULL,
    [CorrespondencePincode]      VARCHAR (6)   NULL,
    [CorrespondenceContactNo]    VARCHAR (15)  NULL,
    [DeliveryDate]               DATETIME      NULL,
    [PrefDeliveryDate]           DATETIME      NULL,
    [BookingEventDate]           DATETIME      NULL,
    [BookingName]                VARCHAR (50)  NULL,
    [BookingMobile]              VARCHAR (50)  NULL,
    [BookingDate]                DATETIME      NULL,
    [EngineNumber]               VARCHAR (100) NULL,
    [InsuranceCoverNumber]       VARCHAR (100) NULL,
    [InvoiceNumber]              VARCHAR (100) NULL,
    [UserIdForDelivery]          INT           NULL,
    [RegistrationNo]             VARCHAR (100) NULL,
    [DeliveryEntryDate]          DATETIME      NULL,
    [ChassisNumber]              VARCHAR (100) NULL,
    [InvoiceDate]                DATE          NULL,
    [Salutation]                 VARCHAR (15)  NULL,
    [LastName]                   VARCHAR (100) NULL,
    [ModelYear]                  VARCHAR (10)  NULL,
    [CarColorId]                 INT           NULL,
    [TC_PaymentModeId]           INT           NULL,
    [TC_OffersId]                INT           NULL,
    [RetailStatus]               INT           NULL,
    [IsOfferClaimed]             BIT           DEFAULT ((0)) NULL,
    [IsPrebook]                  BIT           NULL,
    [PaymentMode]                VARCHAR (50)  NULL,
    [PickupDateTime]             DATETIME      NULL,
    [TC_ActionApplicationId]     INT           NULL,
    [TC_Deals_StockVINId]        INT           NULL,
    [TC_Deals_StockId]           INT           NULL,
    CONSTRAINT [PK_TC_NewCarBookingId] PRIMARY KEY NONCLUSTERED ([TC_NewCarBookingId] ASC),
    CONSTRAINT [DF_TC_NewCarBooking_TC_LeadDispositionId] FOREIGN KEY ([BookingStatus]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarBooking]
    ON [dbo].[TC_NewCarBooking]([ChassisNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarBooking_InvoiceDate]
    ON [dbo].[TC_NewCarBooking]([InvoiceDate] ASC)
    INCLUDE([TC_NewCarBookingId], [TC_NewCarInquiriesId]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarBooking_TC_NewCarInquiriesId]
    ON [dbo].[TC_NewCarBooking]([TC_NewCarInquiriesId] ASC);

