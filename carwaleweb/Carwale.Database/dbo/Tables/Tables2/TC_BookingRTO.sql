CREATE TABLE [dbo].[TC_BookingRTO] (
    [TC_BookingRTO_Id]      INT           IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]      INT           NOT NULL,
    [RTOBy]                 CHAR (6)      NULL,
    [IsRTONOCGiven]         BIT           CONSTRAINT [DF_TC_BookingRTO_IsRTONOCGiven] DEFAULT ((0)) NULL,
    [IsTransferSetGiven]    BIT           CONSTRAINT [DF_TC_BookingRTO_IsTransferSetGiven] DEFAULT ((0)) NULL,
    [IsBankNOCGiven]        BIT           CONSTRAINT [DF_TC_BookingRTO_IsBankNOCGiven] DEFAULT ((0)) NULL,
    [TC_RTO_Id]             INT           NULL,
    [TC_RTOAgent_Id]        INT           NULL,
    [AgentCommisionPaid]    INT           NULL,
    [IsTransferLodged]      BIT           CONSTRAINT [DF_TC_BookingRTO_IsTransferLodged] DEFAULT ((0)) NULL,
    [TransferLodgedDate]    DATE          NULL,
    [IsDocumentDelivered]   BIT           CONSTRAINT [DF_TC_BookingRTO_IsDocumentDelivered] DEFAULT ((0)) NULL,
    [DocumentDeliveredDate] DATE          NULL,
    [Comments]              VARCHAR (200) NULL,
    [EntryDate]             DATETIME      CONSTRAINT [DF_TC_BookingRTO_EntryDate] DEFAULT (getdate()) NULL,
    [IsActive]              BIT           CONSTRAINT [DF_TC_BookingRTO_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]          DATE          NULL,
    [ModifiedBy]            INT           NULL,
    CONSTRAINT [PK_TC_BookingRTO] PRIMARY KEY CLUSTERED ([TC_BookingRTO_Id] ASC),
    CONSTRAINT [UNQ_RTOCarBookingId] UNIQUE NONCLUSTERED ([TC_CarBooking_Id] ASC)
);

