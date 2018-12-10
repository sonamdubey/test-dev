CREATE TABLE [dbo].[Microsite_BookingTransaction] (
    [Id]                   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [DealerId]             NUMERIC (18)   NOT NULL,
    [MakeId]               INT            NULL,
    [ModelId]              INT            NULL,
    [OfferId]              INT            NULL,
    [CustomerName]         VARCHAR (100)  NULL,
    [CustomerMobile]       VARCHAR (20)   NULL,
    [CustomerEmail]        VARCHAR (100)  NULL,
    [CustomerStateId]      INT            NULL,
    [CustomerCityId]       INT            NULL,
    [CustomerAddress]      VARCHAR (1000) NULL,
    [FuelTypeId]           INT            NULL,
    [TransmissionTypeId]   INT            NULL,
    [VersionId]            INT            NULL,
    [VersionPrice]         VARCHAR (15)   NULL,
    [Color]                VARCHAR (20)   NULL,
    [OutletId]             INT            NULL,
    [PaymentType]          INT            NULL,
    [PaymentMode]          INT            NULL,
    [PaymentAmount]        INT            NULL,
    [IsPaymentSuccessfull] BIT            NULL,
    [PGTransactionId]      NUMERIC (18)   NULL,
    [PaymentDate]          DATETIME       NULL,
    [EntryDate]            DATETIME       CONSTRAINT [DF_Microsite_BookingTransaction_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]         DATETIME       NULL,
    [ClientIp]             VARCHAR (20)   NULL,
    [UserAgent]            VARCHAR (50)   NULL,
    [HostName]             VARCHAR (50)   NULL,
    [AutoBizInqId]         BIGINT         NULL,
    [BookingAmount]        INT            NULL,
    [PickUpDate]           DATETIME       NULL,
    [AutoBizResponse]      VARCHAR (500)  NULL,
    CONSTRAINT [PK_Microsite_Bookings] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Microsite_BookingTransaction_ModelId]
    ON [dbo].[Microsite_BookingTransaction]([ModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Microsite_BookingTransaction_VersionId]
    ON [dbo].[Microsite_BookingTransaction]([VersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Microsite_BookingTransaction_DealerId]
    ON [dbo].[Microsite_BookingTransaction]([DealerId] ASC);

