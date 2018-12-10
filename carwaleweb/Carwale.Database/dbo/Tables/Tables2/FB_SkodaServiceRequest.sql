CREATE TABLE [dbo].[FB_SkodaServiceRequest] (
    [Id]                  NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SkodaDealerDetailId] NUMERIC (18)  NOT NULL,
    [DealerName]          VARCHAR (200) NOT NULL,
    [DealerEmail]         VARCHAR (200) NOT NULL,
    [CustomerName]        VARCHAR (100) NOT NULL,
    [CustomerMobile]      NUMERIC (18)  NOT NULL,
    [CustomerEmail]       VARCHAR (100) NOT NULL,
    [CustomerCar]         VARCHAR (50)  NOT NULL,
    [CustomerRegNo]       VARCHAR (50)  NULL,
    [CustomerVINNo]       VARCHAR (50)  NULL,
    [PreferredDate]       DATETIME      NULL
);

