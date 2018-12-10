CREATE TABLE [dbo].[Microsite_DeliveryTime] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [VersionId]     INT          NULL,
    [BookingAmount] VARCHAR (20) NULL,
    [DeliveryTime]  VARCHAR (50) NULL,
    [EntryDate]     DATETIME     NULL,
    [ModifiedDate]  DATETIME     NULL,
    [DealerId]      INT          NULL,
    [CityId]        INT          NULL
);

