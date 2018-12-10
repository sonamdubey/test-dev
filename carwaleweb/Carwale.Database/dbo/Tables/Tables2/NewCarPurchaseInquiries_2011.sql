CREATE TABLE [dbo].[NewCarPurchaseInquiries_2011] (
    [Id]                  NUMERIC (18)   IDENTITY (18404972, 1) NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [CarVersionId]        NUMERIC (18)   NOT NULL,
    [Color]               VARCHAR (50)   NULL,
    [NoOfCars]            INT            NOT NULL,
    [BuyTime]             VARCHAR (50)   NULL,
    [Comments]            VARCHAR (2000) NULL,
    [RequestDateTime]     DATETIME       NOT NULL,
    [IsApproved]          BIT            NOT NULL,
    [IsFake]              BIT            NOT NULL,
    [StatusId]            SMALLINT       NOT NULL,
    [IsForwarded]         BIT            NOT NULL,
    [IsRejected]          BIT            NOT NULL,
    [IsViewed]            BIT            NOT NULL,
    [IsMailSend]          BIT            NOT NULL,
    [TestdriveDate]       VARCHAR (100)  NULL,
    [TestDriveLocation]   VARCHAR (300)  NULL,
    [LatestOffers]        BIT            NULL,
    [ForwardedLead]       BIT            NOT NULL,
    [SourceId]            SMALLINT       NOT NULL,
    [ReqDateTimeDatePart] DATETIME       NULL
);

