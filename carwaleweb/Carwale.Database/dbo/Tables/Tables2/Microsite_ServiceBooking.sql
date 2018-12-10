CREATE TABLE [dbo].[Microsite_ServiceBooking] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [Microsite_CustomerId] INT           NULL,
    [DealerId]             INT           NULL,
    [ModelId]              INT           NULL,
    [MakeYear]             DATE          NULL,
    [RegNumber]            VARCHAR (10)  NULL,
    [KmsDriven]            INT           NULL,
    [PreferredDateTime]    DATETIME      NULL,
    [PickupAddress]        VARCHAR (200) NULL,
    [Comments]             VARCHAR (200) NULL,
    [ServiceCenterId]      INT           NULL,
    [EntryDate]            DATETIME      DEFAULT (getdate()) NULL,
    [ModifiedDate]         DATETIME      NULL,
    [ServiceCompleted]     BIT           NULL,
    [AutobizInqId]         INT           NULL,
    [PaymentAmount]        FLOAT (53)    NULL,
    [PaymentDateTime]      DATETIME      NULL,
    [PGTransactionId]      INT           NULL,
    [IsPaymentDone]        BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

