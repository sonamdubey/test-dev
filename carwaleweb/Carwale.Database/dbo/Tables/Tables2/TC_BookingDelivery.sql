CREATE TABLE [dbo].[TC_BookingDelivery] (
    [TC_BookingDelivery_Id]   INT           IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]        INT           NOT NULL,
    [IsDocDelivered]          BIT           CONSTRAINT [DF_TC_BookingDelivery_IsDocDelivered] DEFAULT ((0)) NOT NULL,
    [IsCarChecked]            BIT           CONSTRAINT [DF_TC_BookingDelivery_IsCarChecked] DEFAULT ((0)) NOT NULL,
    [IsAccessoriesGiven]      BIT           CONSTRAINT [DF_TC_BookingDelivery_IsAccessoriesGiven] DEFAULT ((0)) NOT NULL,
    [TC_BookingWarranties_Id] INT           NOT NULL,
    [TC_BookingServices_Id]   INT           NOT NULL,
    [IsCarDelivered]          BIT           CONSTRAINT [DF_TC_BookingDelivery_IsCarDelivered] DEFAULT ((0)) NOT NULL,
    [DeliveryDate]            DATE          NULL,
    [Comments]                VARCHAR (200) NULL,
    [DeliveryNotes]           VARCHAR (400) NULL,
    [ChassisNumber]           VARCHAR (30)  NULL,
    [LincenseNumber]          VARCHAR (30)  NULL,
    [EntryDate]               DATETIME      CONSTRAINT [DF_TC_BookingDelivery_EntryDate] DEFAULT (getdate()) NULL,
    [IsActive]                BIT           CONSTRAINT [DF_TC_BookingDelivery_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]            DATE          NULL,
    [ModifiedBy]              INT           NULL,
    [EngineNumber]            VARCHAR (30)  NULL,
    CONSTRAINT [PK_TC_BookingDelivery] PRIMARY KEY CLUSTERED ([TC_BookingDelivery_Id] ASC),
    CONSTRAINT [UNQ_DeliveryCarBookingId] UNIQUE NONCLUSTERED ([TC_CarBooking_Id] ASC)
);

