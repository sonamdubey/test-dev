CREATE TABLE [dbo].[TC_BookingRTO_2011] (
    [TC_BookingRTO_Id]      INT           IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]      INT           NOT NULL,
    [RTOBy]                 CHAR (6)      NULL,
    [IsRTONOCGiven]         BIT           NULL,
    [IsTransferSetGiven]    BIT           NULL,
    [IsBankNOCGiven]        BIT           NULL,
    [TC_RTO_Id]             INT           NULL,
    [TC_RTOAgent_Id]        INT           NULL,
    [AgentCommisionPaid]    INT           NULL,
    [IsTransferLodged]      BIT           NULL,
    [TransferLodgedDate]    DATE          NULL,
    [IsDocumentDelivered]   BIT           NULL,
    [DocumentDeliveredDate] DATE          NULL,
    [Comments]              VARCHAR (200) NULL,
    [EntryDate]             DATETIME      NULL,
    [IsActive]              BIT           NOT NULL,
    [ModifiedDate]          DATE          NULL,
    [ModifiedBy]            INT           NULL
);

